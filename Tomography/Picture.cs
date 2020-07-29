namespace Tomography
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Drawing;
    using System.Windows.Forms;
    using Delaunay;
    using FEM;

    /// <summary>
    /// Класс рисования.
    /// </summary>
    public class Picture<T> where T : Triangle<T>
    {
        PictureBox picBox;  // Элемент интерфейса PictureBox.
        Graphics grafics;  // Поверхность для рисования.

        HashSet<Circle> figures;  // Список фигур.
        List<Vertex<T>> points;  // Список точек.
        List<Rib<T>> ribs;  // Список ребер.
        

        /// <summary>
        /// Конструктор для фигур.
        /// </summary>
        /// <param name="picBox">Элемент интерфейса PictureBox.</param>
        /// <param name="figures">Список фигур.</param>
        public Picture(PictureBox picBox, HashSet<Circle> figures)
        {
            this.picBox = picBox;
            this.figures = figures;

            Draw(0);
        }

        /// <summary>
        /// Конструктор для сетки.
        /// </summary>
        /// <param name="picBox">Элемент интерфейса PictureBox.</param>
        /// <param name="points">Список точек.</param>
        /// <param name="ribs">Список ребер.</param>
        public Picture(PictureBox picBox, List<Vertex<T>> points, List<Rib<T>> ribs)
        {
            this.picBox = picBox;
            this.points = points;
            this.ribs = ribs;

            Draw(1);
        }
        
        /// <summary>
        /// Выбор методов построения рисунка в PictureBox.
        /// </summary>
        /// <param name="n">Параметр выбора построения (от 0 до 1).</param>
        public void Draw(int n)
        {
            picBox.Image = new Bitmap(picBox.Width, picBox.Height);
            grafics = Graphics.FromImage(picBox.Image);
            grafics.Clear(picBox.BackColor);

            grafics.ScaleTransform(0.99f, 0.99f); // Доработать масштабирование модели, позиции.
            
            switch (n)
            {
                case (0):
                    DrawFigures();
                    break;
                case (1):
                    DrawRibs();
                    DrawPoints();
                    break;
            }
            
            picBox.Refresh();
        }

        /// <summary>
        /// Построение точек.
        /// </summary>
        private void DrawPoints()
        {
            var potentialHasValue = new SolidBrush(Color.FromArgb(192, 34, 59));
            var myBrash = new SolidBrush(Color.DarkSlateBlue);

            foreach (var point in points)
            {
                var p = InvertY(point);

                if(point.Potential.HasValue)
                    grafics.FillEllipse(potentialHasValue, p.X - 6 / 2, p.Y - 6 / 2, 6, 6);
                else
                    grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
            }
        }

        ///// <summary>
        ///// Построение точек.
        ///// </summary>
        //private void DrawPoints()
        //{
        //    foreach (var point in points)
        //    {
        //        var p = InvertY(point);

        //        if (point.Potential > 90)
        //        {
        //            SolidBrush myBrash = new SolidBrush(Color.FromArgb(255, 0, 0));
        //            grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
        //        }
        //        else if (point.Potential <= 90 && point.Potential > 80)
        //        {
        //            SolidBrush myBrash = new SolidBrush(Color.FromArgb(255, 40, 0));
        //            grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
        //        }
        //        else if (point.Potential <= 80 && point.Potential > 70)
        //        {
        //            SolidBrush myBrash = new SolidBrush(Color.DarkBlue);
        //            grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
        //        }
        //        else if (point.Potential <= 70 && point.Potential > 60)
        //        {
        //            SolidBrush myBrash = new SolidBrush(Color.FromArgb(255, 115, 0));
        //            grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
        //        }
        //        else if (point.Potential <= 60 && point.Potential > 50)
        //        {
        //            SolidBrush myBrash = new SolidBrush(Color.Blue);
        //            grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
        //        }
        //        else if (point.Potential <= 50 && point.Potential > 40)
        //        {
        //            SolidBrush myBrash = new SolidBrush(Color.FromArgb(255, 175, 0));
        //            grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
        //        }
        //        else if (point.Potential <= 40 && point.Potential > 30)
        //        {
        //            SolidBrush myBrash = new SolidBrush(Color.Green);
        //            grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
        //        }
        //        else if (point.Potential <= 30 && point.Potential > 20)
        //        {
        //            SolidBrush myBrash = new SolidBrush(Color.FromArgb(255, 215, 0));
        //            grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
        //        }
        //        else if (point.Potential <= 20 && point.Potential > 10)
        //        {
        //            SolidBrush myBrash = new SolidBrush(Color.Cyan);
        //            grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
        //        }
        //        else if (point.Potential <= 10)
        //        {
        //            SolidBrush myBrash = new SolidBrush(Color.FromArgb(255, 255, 0));
        //            grafics.FillEllipse(myBrash, p.X - 5 / 2, p.Y - 5 / 2, 5, 5);
        //        }
        //    }
        //}

        /// <summary>
        /// Построение ребер.
        /// </summary>
        private void DrawRibs()
        {
            var myPen = new Pen(Color.YellowGreen);
            var myPenBorder = new Pen(Color.DarkSlateBlue);

            foreach (var rib in ribs.Where(r => r.dfdn.HasValue == false || r.sigma.HasValue == false))
            {
                var p1 = InvertY(rib.A);
                var p2 = InvertY(rib.B);

                grafics.DrawLine(myPen, p1.X, p1.Y, p2.X, p2.Y);
            }

            foreach (var rib in ribs.Where(r => r.dfdn.HasValue || r.sigma.HasValue))
            {
                var p1 = InvertY(rib.A);
                var p2 = InvertY(rib.B);

                grafics.DrawLine(myPenBorder, p1.X, p1.Y, p2.X, p2.Y);
            }
        }

        /// <summary>
        /// Построение окружностей.
        /// </summary>
        private void DrawFigures()
        {
            var myPen = new Pen(Color.FromArgb(67, 80, 162), 1);

            foreach (var figure in figures)
            {
                var p = InvertY(new Vertex<T>(figure.Centre.X, figure.Centre.Y));
                grafics.DrawEllipse(myPen, p.X - figure.R, p.Y - figure.R, 2 * figure.R, 2 * figure.R);
            }
        }

        /// <summary>
        /// Инвертирование оси ординат.
        /// </summary>
        public Vertex<T> InvertY(Vertex<T> p)
        {
            return new Vertex<T>(p.X, picBox.Height - p.Y);
        }
    }
}