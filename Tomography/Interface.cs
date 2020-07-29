namespace Tomography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using System.Diagnostics;
    using Delaunay;
    using FEM;
    using Geometry;
    using System.Runtime;


    public partial class MainClass : Form
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainClass()
        { InitializeComponent(); }
        
        AddFigure form;
        Picture picture;  // Графика.
        
        Grid grid;  // Сетка МКЭ.
        FEMethod fem;  // МКЭ.
        
        List<IFigure> figures = new List<IFigure>();  // Список фигур.
        List<Vertex> points;
        List<Rib> ribs;

        /// <summary>
        /// Кнопка расчета МКЭ.
        /// </summary>
        void tSButtonCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                fem = new FEMethod(grid);

                richTBResult.Visible = true;
                groupBSLA.Visible = true;

                // Отображение результатов расчета в точках наблюдения.
                richTBResult.Text = "";
                for (int i = 0; i < 16; i++)
                    richTBResult.Text += "φ" + i.ToString() + " = " + grid.viewPoints[i].Potential.Value.ToString() + "\n";
                
                label26.Text = "Форм-е:  " + Convert.ToString(fem.timeFill.Elapsed);
                label25.Text = "Решение: " + Convert.ToString(fem.timeSol.Elapsed);
                
                // Если максимальное ребро меньше фиксированного значения, то используется цветная картина поля.
                if (ribs.Max(r => r.lenght) < 10)
                {
                    picture.ClearGrafics();
                    picture.DrawPoints(points);
                    picture.DrawRibs(ribs);
                }

                picture.DrawElectrode(grid.viewPoints.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Кнопка построения сетки.
        /// </summary>
        void tSButtonGrid_Click(object sender, EventArgs e)
        {
            var oldMode = GCSettings.LatencyMode;
            System.Runtime.CompilerServices.RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                var size = float.Parse(tBoxSize.Text);

                if (size < 0)
                {
                    MessageBox.Show("Параметры сетки выбраны не верно!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GCSettings.LatencyMode = GCLatencyMode.Batch;

                // ПОСТРОЕНИЕ сетки.
                var time = Stopwatch.StartNew();
                grid = new Grid(figures, size);
                time.Stop();

                // Список ребер и точек для отрисовки.
                ribs =grid.SelectMany(t => t.Ribs).Distinct()/*.Where(rr => rr.sigma.HasValue|| rr.dfdn.HasValue).Where(rr2 => rr2.sigma.Value == 2)*/.ToList();
                points = grid.SelectMany(n => n.Points).Distinct().ToList();

                points.SelectMany(t => t.adjacentTriangles).Distinct();
                label1.Text = "Время:   " + Convert.ToString(time.Elapsed);
                label3.Text = "Треугольники:   " + Convert.ToString(grid.Count());
                label4.Text = "Точки:   " + Convert.ToString(points.Count());

                picture.ClearGrafics();
                picture.DrawRibs(ribs, true);
                Cleaner(5);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                GCSettings.LatencyMode = oldMode;
                GC.Collect(2, GCCollectionMode.Forced);
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Кнопка вызова формы добавления фигур.
        /// </summary>
        void buttonAddFigure_Click(object sender, EventArgs e)
        {
            form = new AddFigure(figures);
            form.DataFigures += DataFigureChange;
            form.Show();
        }
        
        /// <summary>
        /// Изменение данных формы при изменении списка фигур.
        /// </summary>
        void DataFigureChange()
        {
            dataGridViewFigures.Rows.Clear();
            for (int i = 0; i < figures.Count; i++)
                dataGridViewFigures.Rows.Add((i + 1).ToString() + figures[i].ToString());

            // Отображение фигур графически.
            picture = new Picture(pictureB);
            picture.DrawFigures(figures);
        }

        /// <summary>
        /// Кнопка ОЧИСТКИ.
        /// </summary>
        void tSButtonCleaner_Click(object sender, EventArgs e)
        {
            Cleaner(0);
            Cleaner(1);
        }

        /// <summary>
        /// Выборочная очистка, настройка параметров.
        /// </summary>
        /// <param name="i">Индекс блоков.</param>
        void Cleaner(byte i)
        {
            switch (i)
            {
                // Очистка экземпляров.
                case 0:
                    figures.Clear();
                    fem = null;
                    points = null;
                    ribs = null;

                    if (grid != null)
                        grid.Dispose();

                    if (form != null)
                        form.Dispose();

                    dataGridViewFigures.Rows.Clear();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    break;
                // Очистка блоков интерфейса.
                case 1:
                    pictureB.Image = null;
                    tSButtonCalculate.Enabled = false;
                    gBoxInfo.Visible = false;
                    groupBSLA.Visible = false;
                    richTBResult.Visible = false;
                    tBoxSize.Text = "3";
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

        #region Элементы MenuStrip
        /// <summary>
        /// Открыть список фигур.
        /// </summary>
        void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            figures = FileOperations.OpenFigures(openFile);
            if (figures != null)
                DataFigureChange();
        }
        
        /// <summary>
        /// Быстрое сохранение.
        /// </summary>
        void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != null)
                if (grid.viewPoints.Any() || figures.Any())
                {
                    FileOperations.FastWriteData(grid.viewPoints, figures, points.Count);
                    MessageBox.Show("Быстрое сохранение завершено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        /// <summary>
        /// Сохранение списка фигур.
        /// </summary>
        void saveFiguresToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (figures.Any())
            {
                var str = FileOperations.WriteFigures(saveFile, figures);
                if (str.Length != 0)
                    MessageBox.Show(str, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        /// <summary>
        /// Сохранение данных расчета в txt.
        /// </summary>
        void saveDataToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (grid != null)
                if (grid.viewPoints.Any())
                {
                    var str = FileOperations.WriteDataElectrode(saveFile, grid.viewPoints, points.Count);
                    if (str.Length != 0)
                        MessageBox.Show(str, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        /// <summary>
        /// Сохранение данных расчета в Excel.
        /// </summary>
        void saveDataExcelСДатчиковExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != null)
                if (grid.viewPoints.Any())
                {
                    var str = FileOperations.WriteDataElectrodeExcel(saveFile, grid.viewPoints, points.Count);
                    if (str.Length != 0)
                        MessageBox.Show(str, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        /// <summary>
        /// Кнопка Выхода.
        /// </summary>
        void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        /// <summary>
        /// Редактирование данных фигур.
        /// </summary>
        void dataGridViewFigures_Click(object sender, EventArgs e)
        {
            if (figures.Any())
            {
                var n = dataGridViewFigures.CurrentRow.Index;

                if (n <= figures.Count)
                {
                    form = new AddFigure(figures, n);
                    form.DataFigures += DataFigureChange;
                    form.Show();
                }
            }
        }

        /// <summary>
        /// Координаты курсора в PictureBox.
        /// </summary>
        void pictureB_MouseMove(object sender, MouseEventArgs e)
        {
            var p = pictureB.PointToClient(Cursor.Position);
            label6.Text = string.Format("X= {0}  Y= {1}", p.X, pictureB.Height - p.Y);
        }
    }
}