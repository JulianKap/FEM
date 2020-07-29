 namespace Tomography.FEM
{
    using System;
    using System.Collections.Generic;
    using Delaunay;
    using Geometry;


    /// <summary>
    /// Класс построения сетки МКЭ.
    /// </summary>
    public sealed class Grid : IEnumerable<Triangle>, IDisposable
    {
        ICollection<Triangle> grid;  // Сетка конечных элементов.
        List<Vertex> gridPoints;  // Сетка узлов.
        float size;  // Размерность сетки (>0).

        /// <summary>
        /// Узлы (датчики) для отображения результатов.
        /// </summary>
        public List<Vertex> viewPoints;

        /// <summary>
        /// Структурные ребра для триангуляции с ограничениями.
        /// </summary>
        public List<Rib> structRibs { get; private set; }
        

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="figures">Фигуры.</param>
        /// <param name="size">Размерность сетки (>0).</param>
        public Grid(List<IFigure> figures, float size)
        {
            this.size = size;

            gridPoints = new List<Vertex>();
            structRibs = new List<Rib>();

            SetGrid(figures);
        }

        /// <summary>
        /// Построение сетки МКЭ.
        /// </summary>
        /// <param name="figures">Фигуры.</param>
        void SetGrid(List<IFigure> figures)
        {
            // Построение границ фигур (узлы и структурные ребра).
            foreach (var figure in figures)
            {
                List<Rib> ribs;

                if (figure.U1.HasValue && figure.U2.HasValue)
                    gridPoints.AddRange(figure.PointsBorderWithElectrodes(size, out ribs, out viewPoints));
                else
                    gridPoints.AddRange(figure.PointsBorder(size, out ribs));

                structRibs.AddRange(ribs);
            }
            
            grid = new Triangulation(gridPoints, structRibs, figures).triangles;
            gridPoints.Clear();
        }
        
        /// <summary>
        /// Диструтор.
        /// </summary>
        public void Dispose()
        {
            grid.Clear();
            viewPoints.Clear();
            structRibs.Clear();
        }
        

        #region IEnumerable.
        /// <summary>
        /// Реализация интерфейса IEnumerable.
        /// </summary>
        public IEnumerator<Triangle> GetEnumerator()
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