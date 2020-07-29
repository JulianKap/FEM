 namespace Tomography.FEM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Delaunay;

    /// <summary>
    /// Класс построения сетки МКЭ.
    /// </summary>
    public sealed class Grid : IEnumerable<FiniteElement>
    {
        ICollection<FiniteElement> grid;  // Сетка конечных элементов.
        List<Vertex<FiniteElement>> gridPoints;  // Сетка узлов.
        
        float size;  // Размерность сетки (>0).
        int border;  // Плотность сетки на границах (>0).

        /// <summary>
        /// Узлы (датчики) для отображения результатов.
        /// </summary>
        public List<Vertex<FiniteElement>> viewPoints { get; private set; }

        /// <summary>
        /// Структурные ребра для триангуляции с ограничениями.
        /// </summary>
        public HashSet<Rib<FiniteElement>> structRibs { get; private set; }
        

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="figures">Фигуры.</param>
        /// <param name="size">Размерность сетки (>0).</param>
        /// <param name="border">Плотность сетки на границах (>0).</param>
        public Grid(HashSet<Circle> figures, float size, int border = 0)
        {
            this.size = size;
            this.border = border;

            gridPoints = new List<Vertex<FiniteElement>>();
            structRibs = new HashSet<Rib<FiniteElement>>();

            SetGrid(figures);
        }

        /// <summary>
        /// Построение сетки МКЭ.
        /// </summary>
        /// <param name="figures">Фигуры.</param>
        private void SetGrid(HashSet<Circle> figures)
        {
            // Построение границ фигур (узлы и структурные ребра).
            foreach (var figure in figures)
            {
                gridPoints.Add(new Vertex<FiniteElement>(figure.Centre.X, figure.Centre.Y));

                if(figure.U1.HasValue)
                    PointsRoundWhisElectrodes(figure);
                else
                    PointsRound(figure);
            }

            // Триангуляция.
            grid = (new Triangulation<FiniteElement>(gridPoints, structRibs, true, border)).ToList();

            // Задание значения электропроводности для конечных элементов.
            foreach (var figure in figures)
                foreach (var p in grid)
                    if (figure.HittingTheCircleInside(p.centre))
                    {
                        p.Permeability = figure.Permeability;
                        continue;
                    }
        }

        /// <summary>
        /// Кольцо точек фигуры (окружность).
        /// </summary>
        /// <param name="O">Окружность.</param>
        private void PointsRound(Circle O)
        {
            // Радиус.
            var R = O.R;

            var step = (float)(Math.Acos((2 * R * R - size * size) / (2 * R * R)) * 180 / Math.PI);
            float n = 0;

            var field = new List<Vertex<FiniteElement>>();

            while (n < 360)
            {
                field.Add(PointInRound(O, n));
                n += step;
            }

            SetStructureRib(O, field);
            gridPoints.AddRange(field);
        }

        /// <summary>
        /// Кольцо точек фигуры (окружность) с датчиками.
        /// </summary>
        /// <param name="O">Окружность.</param>
        private void PointsRoundWhisElectrodes(Circle O)
        {
            // Радиус.
            var R = O.R;

            var step = (float)(Math.Acos((2 * R * R - size * size) / (2 * R * R)) * 180 / Math.PI);
            var stepElectrode = 22.5f; // 16 точек на окружности.
            var bo = size / 10;
            
            float n = 90;
            float nElectrode = 90;

            viewPoints = new List<Vertex<FiniteElement>>();
            var field = new List<Vertex<FiniteElement>>();
            
            while (n < 450)
            {
                n += step;
                bool add = false;
                
                if (n > nElectrode - bo)
                {
                    n = nElectrode;
                    nElectrode += stepElectrode;

                    if (nElectrode > 450)
                        break;

                    add = true;
                }

                var point = PointInRound(O, n);
                field.Add(point);

                if (add)
                {
                    viewPoints.Add(point);

                    if (n == 90)
                        point.Potential = O.U1;
                    else if (n == 270)
                        point.Potential = O.U2;
                }
            }
            
            SetStructureRib(O, field);
            gridPoints.AddRange(field);
        }
        
        /// <summary>
        /// Точка на окружности
        /// </summary>
        /// <param name="O">Окружность.</param>
        /// <param name="n"></param>
        /// <returns></returns>
        private Vertex<FiniteElement> PointInRound(Circle O, double n)
        {
            // Угол в радианах.
            var angle = n * Math.PI / 180;
            var x = O.Centre.X + O.R * Math.Cos(angle);
            var y = O.Centre.Y + O.R * Math.Sin(angle);

            return new Vertex<FiniteElement>((float)x, (float)y);
        }
        
        /// <summary>
        /// Добавление структурных ребер.
        /// </summary>
        /// <param name="O">Окружность.</param>
        /// <param name="points">Список узлов структурных ребер.</param>
        private void SetStructureRib(Circle O, List<Vertex<FiniteElement>> points)
        {
            var n = points.Count;

            for (int i1 = 0; i1 < n; i1++)
            {
                var i2 = i1 + 1;
                if (i2 == n)  i2 = 0;

                var rib = new Rib<FiniteElement>(points[i1], points[i2], null, null);
                
                // Добавление граничных условий.
                if (O.dfdn.HasValue)
                    rib.dfdn = O.dfdn.Value;
                else if (O.sigma.HasValue)
                    rib.sigma = O.sigma.Value;

                structRibs.Add(rib);
            }
        }
        
        #region IEnumerable.
        /// <summary>
        /// Реализация интерфейса IEnumerable.
        /// </summary>
        public IEnumerator<FiniteElement> GetEnumerator()
        {
            return grid.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}