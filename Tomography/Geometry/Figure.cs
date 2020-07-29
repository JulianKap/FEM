namespace Tomography.Geometry
{
    using System;
    using System.Collections.Generic;
    using Delaunay;

    /// <summary>
    /// Абстрактный класс фигур.
    /// </summary>
    [Serializable]
    public abstract class Figure: IFigure
    {
        /// <summary>
        /// Проводимость среды.
        /// </summary>
        public float Permeability { get; set; }

        /// <summary>
        /// Нормальная производная на границе.
        /// </summary>
        public float? dfdn { get; set; }
        
        /// <summary>
        /// Входное напряжение U1.
        /// </summary>
        public float? U1 { get; set; }

        /// <summary>
        /// Выходное напряжение U2.
        /// </summary>
        public float? U2 { get; set; }
        
        /// <summary>
        /// Проверка попадания точки внутрь фигуры.
        /// </summary>
        /// <param name="p">Точка.</param>
        public abstract bool HittingTheInside(Vertex p);

        /// <summary>
        /// Точки границы фигуры.
        /// </summary>
        /// <param name="size">Дискретизация границы.</param>
        /// <param name="structRibs">Структурные ребра границы фигуры.</param>
        /// <returns>Список точек.</returns>
        public abstract List<Vertex> PointsBorder(float size, out List<Rib> structRibs);

        /// <summary>
        /// Кольцо точек фигуры с датчиками.
        /// </summary>
        /// <param name="size">Дискретизация границы.</param>
        /// <param name="viewPoints">Список точек наблюдения.</param>
        /// <param name="structRibs">Структурные ребра границы фигуры.</param>
        /// <returns>Список точек.</returns>
        public abstract List<Vertex> PointsBorderWithElectrodes(float size, out List<Rib> structRibs, out List<Vertex> viewPoints);

        /// <summary>
        /// Добавление структурных ребер.
        /// </summary>
        /// <param name="points">Список узлов структурных ребер.</param>
        protected virtual List<Rib> SetRibs(List<Vertex> points)
        {
            var structRibs = new List<Rib>();
            var n = points.Count;

            for (int i0 = 0; i0 < n; i0++)
            {
                var i1 = i0 + 1;
                
                if (i1 == n) i1 = 0;

                var rib = new Rib(points[i0], points[i1], null, null);

                // Добавление граничных условий.
                if (dfdn.HasValue)
                    rib.dfdn = dfdn.Value;

                structRibs.Add(rib);
            }

            return structRibs;
        }

        /// <summary>
        /// Описание фигуры.
        /// </summary>
        /// <returns>Текстовое предстваление фигуры.</returns>
        public abstract override string ToString();
    }
}