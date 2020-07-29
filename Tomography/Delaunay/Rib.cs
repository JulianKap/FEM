namespace Tomography.Delaunay
{
    /// <summary>
    /// Класс ребер.
    /// </summary>
    public class Rib<T> where T: Triangle<T>
    {
        /// <summary>
        /// Точка А.
        /// </summary>
        public Vertex<T> A { get; }

        /// <summary>
        /// Точка Б.
        /// </summary>
        public Vertex<T> B { get; }

        /// <summary>
        /// Первый треугольник, содержащий ребро.
        /// </summary>
        public T T1 { get; private set; }

        /// <summary>
        /// Второй треугольник, содержащий ребро.
        /// </summary>
        public T T2 { get; private set; }

        /// <summary>
        /// Нормальная производная на границе.
        /// </summary>
        public float? dfdn { get; set; }

        /// <summary>
        /// Заряд на границе двух сред.
        /// </summary>
        public float? sigma { get; set; }

        /// <summary>
        /// Длина ребра.
        /// </summary>
        public float Lenght
        {
            get
            {
                if (!lenght.HasValue)
                    lenght = A.VectorLength(B);

                return lenght.Value;
            }
        }

        private float? lenght;
        

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public Rib()
        { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="A">Точка А.</param>
        /// <param name="B">Точка Б.</param>
        /// <param name="T1">Первый треугольник.</param>
        /// <param name="T2">Второй треугольник.</param>
        public Rib(Vertex<T> A, Vertex<T> B, T T1, T T2)
        {
            this.A = A;
            this.B = B;
            this.T1 = T1;
            this.T2 = T2;
        }
        
        /// <summary>
        /// Обновление треугольников для ребра.
        /// /Треугольник 1\ |ребро| /Треугольник 2\
        /// </summary>
        /// <param name="old">Старый треугольник.</param>
        /// <param name="New">Новый треугольник.</param>
        public void UpdateTriangle(T old, T New)
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
        public T GetTriangleNeighbor(T T)
        {
            if (T == T1)
                return T2;

            return T1;
        }

        /// <summary>
        /// Добавление граничных условий
        /// </summary>
        /// <param name="rib"></param>
        public void SetBorderValue(Rib<T> rib)
        {
            if (rib.dfdn.HasValue)
                dfdn = rib.dfdn.Value;
            else if (rib.sigma.HasValue)
                sigma = rib.sigma.Value;
        }
    }
}