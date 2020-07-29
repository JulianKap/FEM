namespace Tomography.Geometry
{
    using System.Collections.Generic;
    using Delaunay;

    /// <summary>
    /// Интерфейс фигур.
    /// </summary>
    public interface IFigure
    {
        /// <summary>
        /// Проводимость среды.
        /// </summary>
        float Permeability { get; set; }

        /// <summary>
        /// Нормальная производная на границе.
        /// </summary>
        float? dfdn { get; set; }

        /// <summary>
        /// Входное напряжение U1.
        /// </summary>
        float? U1 { get; set; }

        /// <summary>
        /// Выходное напряжение U2.
        /// </summary>
        float? U2 { get; set; }

        /// <summary>
        /// Проверка попадания точки внутрь фигуры.
        /// </summary>
        /// <param name="p">Точка.</param>
        bool HittingTheInside(Vertex p);

        /// <summary>
        /// Точки границы фигуры.
        /// </summary>
        /// <param name="size">Дискретизация границы.</param>
        /// <param name="structRibs">Структурные ребра границы фигуры.</param>
        /// <returns>Список точек.</returns>
        List<Vertex> PointsBorder(float size, out List<Rib> structRibs);

        /// <summary>
        /// Кольцо точек фигуры с датчиками.
        /// </summary>
        /// <param name="size">Дискретизация границы.</param>
        /// <param name="viewPoints">Список точек наблюдения.</param>
        /// <param name="structRibs">Структурные ребра границы фигуры.</param>
        /// <returns>Список точек.</returns>
        List<Vertex> PointsBorderWithElectrodes(float size, out List<Rib> structRibs, out List<Vertex> viewPoints);

        /// <summary>
        /// Описание фигуры.
        /// </summary>
        /// <returns>Текстовое предстваление фигуры.</returns>
        string ToString();
    }
}
