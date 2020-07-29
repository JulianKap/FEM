namespace Tomography
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Drawing;
    using System.Windows.Forms;
    using Delaunay;
    using Geometry;

    /// <summary>
    /// Класс рисования.
    /// </summary>
    public class Picture
    {
        PictureBox picBox;  // Элемент интерфейса PictureBox.
        Graphics grafics;  // Поверхность для рисования.

        
        /// <summary>
        /// Конструктор для фигур.
        /// </summary>
        /// <param name="picBox">Элемент интерфейса PictureBox.</param>
        public Picture(PictureBox picBox)
        {
            this.picBox = picBox;

            this.picBox.Image = new Bitmap(picBox.Width, picBox.Height);
            grafics = Graphics.FromImage(this.picBox.Image);
            ClearGrafics();

            grafics.ScaleTransform(0.99f, 0.99f); // Доработать масштабирование модели, позиции.
        }
        
        /// <summary>
        /// Построение точек - датчиков.
        /// </summary>
        /// <param name="viewPoints">Список датчиков.</param>
        public void DrawElectrode(List<Vertex> viewPoints)
        {
            var myBrash = new SolidBrush(Color.DarkSlateBlue);

            foreach (var point in viewPoints)
            {
                var p = InvertY(point);
                grafics.FillEllipse(myBrash, p.X - 6 / 2, p.Y - 6 / 2, 6, 6);
            }

            myBrash.Dispose();
            picBox.Refresh();
        }

        /// <summary>
        /// Построение точек (Цветовая картина поля).
        /// </summary>
        /// <param name="points">Список точек.</param>
        public void DrawPoints(List<Vertex> points)
        {
            var colors = getSpectrum();

            int n = 3; // Размер точки.

            var minPotencial = points.Min(p => p.Potential);
            var step = (points.Max(p => p.Potential) - minPotencial) / 20;
            
            var range = step + minPotencial;  // Счетчик переключения цвета.
            int i = 0;

            foreach (var point in points.OrderBy(r => r.Potential))
            {
                var p = InvertY(point);
                
                var myBrash = new SolidBrush(colors[i]);
                grafics.FillEllipse(myBrash, p.X - n / 2, p.Y - n / 2, n, n);
                
                if (point.Potential > range)
                {
                    range += step;
                    i++;
                }

                myBrash.Dispose();
            }
            
            picBox.Refresh();
        }

        /// <summary>
        /// Построение ребер.
        /// </summary>
        /// <param name="ribs">Список ребер.</param>
        /// <param name="full">False- рисовать ребра границ, true - рисовать все ребра.</param>
        public void DrawRibs(List<Rib> ribs, bool full = false)
        {
            var myPen = new Pen(Color.YellowGreen);
            var myPenBorder = new Pen(Color.DarkSlateBlue);

            if (full)
                foreach (var rib in ribs.Where(r => r.hasStruct == false))
                {
                    var p1 = InvertY(rib.A);
                    var p2 = InvertY(rib.B);

                    grafics.DrawLine(myPen, p1.X, p1.Y, p2.X, p2.Y);
                }

            foreach (var rib in ribs.Where(r => r.hasStruct))
            {
                var p1 = InvertY(rib.A);
                var p2 = InvertY(rib.B);

                grafics.DrawLine(myPenBorder, p1.X, p1.Y, p2.X, p2.Y);
            }

            myPen.Dispose();
            myPenBorder.Dispose();
            picBox.Refresh();
        }

        /// <summary>
        /// Построение окружностей.
        /// </summary>
        /// <param name="figures">Список фигур.</param>
        public void DrawFigures(List<IFigure> figures)
        {
            var myPen = new Pen(Color.FromArgb(67, 80, 162), 1);
            
            foreach (var figure in figures)
            {
                if (figure is Circle)
                {
                    var circle = figure as Circle;

                    var p = InvertY(circle.Centre);
                    grafics.DrawEllipse(myPen, p.X - circle.R, p.Y - circle.R, 2 * circle.R, 2 * circle.R);
                }
            }

            myPen.Dispose();
            picBox.Refresh();
        }
        
        /// <summary>
        /// Очистка формы
        /// </summary>
        public void ClearGrafics()
        {
            grafics.Clear(picBox.BackColor);
        }

        /// <summary>
        /// Инвертирование оси ординат.
        /// </summary>
        Vertex InvertY(Vertex p)
        {
            var h = picBox.Height;
            return new Vertex(p.X, h - p.Y);
        }

        /// <summary>
        /// Массив цветового спектра.
        /// </summary>
        Color[] getSpectrum()
        {
            var colors = new Color[] {
            Color.FromArgb( 125, 205, 204),
            Color.FromArgb( 111, 198, 164),
            Color.FromArgb( 105, 193, 130),
            Color.FromArgb( 102, 190, 95),
            Color.FromArgb( 113, 191, 77),
            Color.FromArgb( 137, 197, 64),
            Color.FromArgb( 161, 204, 58),
            Color.FromArgb( 186, 212, 50 ),
            Color.FromArgb( 213, 223, 40),
            Color.FromArgb( 240, 234, 39),
            Color.FromArgb( 248, 239, 35),
            Color.FromArgb( 255, 218, 25),
            Color.FromArgb( 253, 191, 26),
            Color.FromArgb( 249, 165, 31),
            Color.FromArgb( 246, 143, 48),
            Color.FromArgb( 244, 120, 74),
            Color.FromArgb( 240, 98, 103),
            Color.FromArgb( 238, 70, 142),
            Color.FromArgb( 198, 67, 152),
            Color.FromArgb( 166, 70, 154),
            };

            return colors;
        }
    }
}