namespace Tomography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using System.Diagnostics;
    using Delaunay;
    using FEM;

    public partial class MainClass : Form
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainClass()
        { InitializeComponent(); }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Пример.
            figures.Add(new Circle(new Vertex<FiniteElement>(300, 250), 250, 2000, 0, 100, 0));
            figures.Add(new Circle(new Vertex<FiniteElement>(360, 320), 80, 10, 0));
        }

        HashSet<Circle> figures = new HashSet<Circle>();  // Список фигур.
        Grid grid;  // Сетка МКЭ.
        FEMethod fem;  // МКЭ.


        /// <summary>
        /// Кнопка расчета МКЭ.
        /// </summary>
        private void tSButtonCalculate_Click(object sender, EventArgs e)
        {
            fem = new FEMethod(grid);

            richTBResult.Visible = true;
            groupBSLA.Visible = true;
            

            // Отображение результатов расчета в точка наблюдения.
            string str = "";
            for (int i = 0; i < 16; i++)
                str += "φ" + i.ToString() + " = " + grid.viewPoints[i].Potential.Value.ToString() + "\n";

            richTBResult.Text = str;

            label26.Text = "Форм-е:  " + Convert.ToString(fem.timeFill.Elapsed);
            label25.Text = "Решение: " + Convert.ToString(fem.timeSol.Elapsed);
        }

        /// <summary>
        /// Кнопка построения сетки.
        /// </summary>
        private void tSButtonGrid_Click(object sender, EventArgs e)
        {
            try
            {
                var size = float.Parse(tBoxSize.Text);

                if (size < 0)
                {
                    MessageBox.Show("Параметры сетки выбраны не верно!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var border = Convert.ToInt16(tBoxBorder.Text);

                // ПОСТРОЕНИЕ сетки.
                var time = Stopwatch.StartNew();
                grid = new Grid(figures, size, border);
                time.Stop();

                // Список ребер и точек для отрисовки.
                var ribs = grid.SelectMany(t => t.Ribs).Distinct()/*.Where(rr => rr.sigma.HasValue|| rr.dfdn.HasValue).Where(rr2 => rr2.sigma.Value == 2)*/.ToList();
                var points = grid.SelectMany(n => n.Points).Distinct().ToList();

                // ОТРИСОВКА сетки.
                new Picture<FiniteElement>(pictureB, grid.viewPoints, ribs);

                //new Picture(pictureB, grid.viewPoints);
                label1.Text = "Время:   " + Convert.ToString(time.Elapsed);
                label3.Text = "Треугольники:   " + Convert.ToString(grid.Count());
                label4.Text = "Точки:   " + Convert.ToString(points.Count());

                Cleaner(5);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        
        /// <summary>
        /// Кнопка добавления фигур.
        /// </summary>
        private void butAddFigure_Click(object sender, EventArgs e)
        {
            try
            {
                // Центр, радиус, электропроводность и граничные условия окружности.
                var centre = new Vertex<FiniteElement>(float.Parse(tBoxCentreX.Text), float.Parse(tBoxCentreY.Text));
                var radius = float.Parse(tBoxRadius.Text);
                var perm = float.Parse(tBoxPermeability.Text);
                var borderValue = float.Parse(tBoxBorderValue.Text);
                
                if (cBoxMainFigure.Checked && cBoxMainFigure.Enabled == true)
                {
                    // Входное, выходное напряжение и количество электродов (сторона).
                    var U1 = float.Parse(tBoxU1.Text);
                    var U2 = float.Parse(tBoxU2.Text);
                    
                    figures.Add(new Circle(centre, radius, perm, borderValue, U1, U2));
                    
                    Cleaner(3, 4);
                }
                else
                    figures.Add(new Circle(centre, radius, perm, borderValue));

                Cleaner(2, 6);
                // Отображение фигур графически.
                new Picture<FiniteElement>(pictureB, figures);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        
        /// <summary>
        /// Кнопка ОЧИСТКИ.
        /// </summary>
        private void tSButtonCleaner_Click(object sender, EventArgs e)
        {
                Cleaner(0, 1, 2, 3);
        }

        /// <summary>
        /// Выборочная очистка, настройка параметров.
        /// </summary>
        /// <param name="block">Индекс блоков: очистка (от 0 до 3), управление элементами(от 4 до 6).</param>
        private void Cleaner(params int[] block)
        {
            foreach (var b in block)
                switch (b)
                {
                    // Очистка экземпляров.
                    case 0:
                        figures.Clear();
                        grid = null;
                        fem = null;
                        break;
                    // Очистка блоков интерфейса.
                    case 1:
                        pictureB.Image = null;
                        tSButtonCalculate.Enabled = false;
                        cBoxMainFigure.Enabled = true;
                        cBoxMainFigure.Checked = true;
                        gBoxInfo.Visible = false;
                        gBoxModel.Visible = true;
                        groupBSLA.Visible = false;
                        richTBResult.Visible = false;
                        break;
                    // Очистка параметров фигуры.
                    case 2:
                        tBoxCentreX.Text = "";
                        tBoxCentreY.Text = "";
                        tBoxRadius.Text = "";
                        tBoxPermeability.Text = "";
                        tBoxBorder.Text = "0";
                        tBoxSize.Text = "25";
                        tBoxBorderValue.Text = "0";
                        label22.Text = "δφ/δn =";
                        break;
                    // Очистка модели.
                    case 3:
                        tBoxU1.Text = "";
                        tBoxU2.Text = "";
                        break;
                    // Отключение модели.
                    case 4:
                        cBoxMainFigure.Enabled = false;
                        cBoxMainFigure_CheckedChanged(null, null);
                        break;
                        
                    case 5:
                        tSButtonCalculate.Enabled = true;
                        gBoxInfo.Visible = true;
                        break;
                    case 6:
                        tSButtonCalculate.Enabled = false;
                        gBoxInfo.Visible = false;
                        break;
                }
        }

        /// <summary>
        /// Выбор главной модели.
        /// </summary>
        private void cBoxMainFigure_CheckedChanged(object sender, EventArgs e)
        {
            if (gBoxModel.Visible == true)
            {
                gBoxModel.Visible = false;
                label22.Text = "σ =";
                Cleaner(3);
            }
            else
            {
                gBoxModel.Visible = true;
                label22.Text = "δφ/δn ="; 
            }
        }

        #region Элементы MenuStrip
        /// <summary>
        /// Открыть список точек
        /// </summary>
        private void списокТочекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //points = FileOperations.OpenPoints(openFile);

            //new Picture(pictureB, points).Draw(0);
        }

        /// <summary>
        /// Открыть коллекцию треугольников
        /// </summary>
        private void списокТреугольниковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //oldDelaunay = FileOperations.OpenCollectionTriangles(openFile);
            //points = oldDelaunay.SelectMany(n => n.Points).Distinct().ToList();
            
            //new Picture(pictureB, points, oldDelaunay.SelectMany(t => t.Ribs).Distinct().ToList()).Draw(2);
        }

        /// <summary>
        /// Быстрое сохранение (список точек и треугольников)
        /// </summary>
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FileOperations.FastWriteData(points, oldDelaunay);
        }

        /// <summary>
        /// Сохранить список точек
        /// </summary>
        private void списокТочекToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //FileOperations.WriteDataPoints(saveFile, points);
        }

        /// <summary>
        /// Сохранить коллекцию
        /// </summary>
        private void списокТреугольниковToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //FileOperations.WriteDataTriangles(saveFile, oldDelaunay);
        }

        /// <summary>
        /// Кнопка Выхода
        /// </summary>
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //сохранитьToolStripMenuItem_Click(null, null);
            //this.Close();
        }
        #endregion
        
        /// <summary>
        /// Координаты точки в PictureBox.
        /// </summary>
        private void pictureB_MouseMove(object sender, MouseEventArgs e)
        {
            // Инвертировать Y.
            //label6.Text = pictureB.PointToClient(new Point(Cursor.Position.X, pictureB.Height - Cursor.Position.Y)).ToString();
        }
    }
}