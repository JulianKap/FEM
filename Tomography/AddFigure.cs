namespace Tomography
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Delaunay;
    using Geometry;

    /// <summary>
    /// Класс добавления фигур с формы.
    /// </summary>
    public partial class AddFigure : Form
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="figures">Список фигур.</param>
        /// <param name="i">Индекс фигуры в списке.</param>
        public AddFigure(List<IFigure> figures, int i = -1)
        {
            InitializeComponent();
            this.figures = figures;
            this.i = i;
            
            if (i == -1)
            {
                CheckFigure();
            }
            else
            {
                var figure = figures[i];

                this.Text = "Изменение фигуры";
                butAddFigure.Text = "Изменить фигуру";
                buttonDelete.Text = "Удалить";

                if (figure is Circle)
                {
                    var circl = figure as Circle;

                    tBoxCentreX.Text = circl.Centre.X.ToString();
                    tBoxCentreY.Text = circl.Centre.Y.ToString();
                    tBoxRadius.Text = circl.R.ToString();
                }
                
                tBoxPermeability.Text = figure.Permeability.ToString();
                
                if (figure.dfdn.HasValue)  tBoxBorderValue.Text = figure.dfdn.Value.ToString();
                else                       Cleaner(2);

                if (figure.U1.HasValue)  tBoxU1.Text = figure.U1.Value.ToString();

                if (figure.U2.HasValue)  tBoxU2.Text = figure.U2.Value.ToString();
            }
        }

        /// <summary>
        /// Делегат.
        /// </summary>
        public delegate void MethodContainer();

        /// <summary>
        /// Событие для изменение данных главной формы.
        /// </summary>
        public event MethodContainer DataFigures;
        
        List<IFigure> figures;  // Список фигур.
        int i;
        

        /// <summary>
        /// Кнопка добавления фигур.
        /// </summary>
        void butAddFigure_Click(object sender, EventArgs e)
        {
            try
            {
                // Центр, радиус, электропроводность и граничные условия окружности.
                var centre = new Vertex(float.Parse(tBoxCentreX.Text), float.Parse(tBoxCentreY.Text));
                var radius = float.Parse(tBoxRadius.Text);

                var circle = new Circle(centre, radius);
                circle.Permeability = float.Parse(tBoxPermeability.Text);
                
                if (cBoxMainFigure.Checked)
                {
                    // Входное, выходное напряжение и нормальная производная.
                    circle.U1 = float.Parse(tBoxU1.Text);
                    circle.U2 = float.Parse(tBoxU2.Text);
                    circle.dfdn = float.Parse(tBoxBorderValue.Text);

                    Cleaner(1);
                    Cleaner(2);
                }
                
                if (i == -1)
                {
                    figures.Add(circle);
                    Cleaner(0);
                    DataFigures();
                }
                else
                {
                    figures[i] = circle;
                    DataFigures();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Кнопка удаления.
        /// </summary>
        void buttonDelete_Click(object sender, EventArgs e)
        {
            if (i != -1)
            {
                figures.RemoveAt(i);
                DataFigures();
            }

            Close();
        }

        /// <summary>
        /// Выбор главной модели.
        /// </summary>
        void cBoxMainFigure_CheckedChanged(object sender, EventArgs e)
        {
            if (tablePanelModel.Enabled == true)
            {
                tablePanelModel.Enabled = false;
                tBoxBorderValue.Enabled = false;
                Cleaner(1);
            }
            else
            {
                tablePanelModel.Enabled = true;
                tBoxBorderValue.Enabled = true;
            }
        }

        /// <summary>
        /// Проверка списка фигур на наличие главной.
        /// </summary>
        void CheckFigure()
        {
            foreach (var figure in figures)
                if (figure.U1.HasValue && figure.U2.HasValue)
                {
                    Cleaner(2);
                    break;
                }
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        /// <param name="i">Номер блока.</param>
        void Cleaner(byte i)
        {
            switch (i)
            {
                // Очистка элементов формы.
                case 0:
                    tBoxCentreX.Text = "";
                    tBoxCentreY.Text = "";
                    tBoxRadius.Text = "";
                    tBoxPermeability.Text = "";
                    tBoxBorderValue.Text = "0";
                    label22.Text = "δφ/δn =";
                    break;
                // Очистка модели.
                case 1:
                    tBoxU1.Text = "";
                    tBoxU2.Text = "";
                    break;
                // Отключение модели.
                case 2:
                    cBoxMainFigure.Checked = false;
                    cBoxMainFigure.Enabled = false;
                    break;
            }
        }
    }
}