namespace Tomography.Delaunay
{
    using System;

    /// <summary>
    /// Класс ребер.
    /// </summary>
    [Serializable]
    public class Rib
    {
        /// <summary>
        /// Точка А.
        /// </summary>
        public Vertex A { get; }

        /// <summary>
        /// Точка Б.
        /// </summary>
        public Vertex B { get; }

        /// <summary>
        /// Первый треугольник, содержащий ребро.
        /// </summary>
        public Triangle T1 { get; private set; }

        /// <summary>
        /// Второй треугольник, содержащий ребро.
        /// </summary>
        public Triangle T2 { get; private set; }

        /// <summary>
        /// Нормальная производная на границе.
        /// </summary>
        public float? dfdn { get; set; }

        /// <summary>
        /// Длина ребра.
        /// </summary>
        public float lenght { get; }

        /// <summary>
        /// Структурное ребро.
        /// </summary>
        public bool hasStruct { get; set; }


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="A">Точка А.</param>
        /// <param name="B">Точка Б.</param>
        /// <param name="T1">Первый треугольник.</param>
        /// <param name="T2">Второй треугольник.</param>
        public Rib(Vertex A, Vertex B, Triangle T1, Triangle T2)
        {
            this.A = A;
            this.B = B;
            this.T1 = T1;
            this.T2 = T2;

            lenght = Vertex.VectorLength(A, B);
            hasStruct = false;
        }

        /// <summary>
        /// Обновление треугольников для ребра.
        /// /Треугольник 1\ |ребро| /Треугольник 2\
        /// </summary>
        /// <param name="old">Старый треугольник.</param>
        /// <param name="New">Новый треугольник.</param>
        public void UpdateTriangle(Triangle old, Triangle New)
        {
            if (T1 == old)
                T1 = New;
            else
                T2 = New;
        }

        /// <summary>
        /// Нахождение соседнего треугольника по ребру.
        /// </summary>
        /// <param name="T">Треугольник, для которого ищется соседний по ребру.</param>
        /// <returns>Возвращение соседнего треугольника.</returns>
        public Triangle GetTriangleNeighbor(Triangle T)
        {
            return T == T1 ? T2 : T1;
        }

        /// <summary>
        /// Добавление граничных условий (копирование граничных условий другого ребра).
        /// </summary>
        /// <param name="rib">Ребро с граничными условиями.</param>
        public void SetBorderValue(Rib rib)
        {
            if (rib.dfdn.HasValue)
                dfdn = rib.dfdn.Value;
        }
    }
}