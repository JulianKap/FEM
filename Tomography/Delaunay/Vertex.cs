namespace Tomography.Delaunay
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;


    /// <summary>
    /// Класс вершин (точек в двумерном пространстве).
    /// </summary>
    [Serializable]
    public  class Vertex : IEquatable<Vertex>, ISerializable
    {
        /// <summary>
        /// Координата точки по X.
        /// </summary>
        public float X { get; protected set; }

        /// <summary>
        /// Координата точки по Y.
        /// </summary>
        public float Y { get; protected set; }

        /// <summary>
        /// Значение потенциала.
        /// </summary>
        public double? Potential { get; set; }

        /// <summary>
        /// Идентификатор точки.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Соседние треугольники.
        /// </summary>
        public List<Triangle> adjacentTriangles { get; protected set; }


        /// <summary>
        /// Пустой констуктор.
        /// </summary>
        public Vertex() : this(0, 0)
        { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="X">Координата по X.</param>
        /// <param name="Y">Координата по Y.</param>
        public Vertex(float X, float Y)
        {
            this.X = X;
            this.Y = Y;

            adjacentTriangles = new List<Triangle>();
        }

        /// <summary>
        /// Конструктор десериализации объекта.
        /// </summary>
        /// <param name="info">Данные.</param>
        /// <param name="context">Источник.</param>
        public Vertex(SerializationInfo info, StreamingContext context)
        {
            this.X = (float)info.GetValue("X", typeof(float));
            this.Y = (float)info.GetValue("Y", typeof(float));

            adjacentTriangles = new List<Triangle>();
        }
        
        /// <summary>
        /// Нахождение точки пересечения двух векторов.
        /// </summary>
        /// <param name="p1">Координата 1.</param>
        /// <param name="p2">Координата 2.</param>
        /// <param name="p3">Координата 3.</param>
        /// <param name="p4">Координата 4.</param>
        /// <returns>Возвращается точка пересечения, или null в случае, если вектора не пересекаются.</returns>
        public static Vertex PointCross(Vertex p1, Vertex p2, Vertex p3, Vertex p4)
        {
            var d = Pseudoscalar(p2, p1, p3, p4);
            
            var ta = Pseudoscalar(p3, p1, p3, p4) / d;
            var tb = Pseudoscalar(p2, p1, p3, p1) / d;
            
            return ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1 
                ? new Vertex(p1.X + ta * (p2.X - p1.X), p1.Y + ta * (p2.Y - p1.Y)) 
                : null;
        }

        /// <summary>
        /// Псевдоскалярное векторное произведение.
        /// </summary>
        public static Func<Vertex, Vertex, Vertex, Vertex, float> Pseudoscalar = (p1, p2, p3, p4) => 
                           (p2.X - p1.X) * (p4.Y - p3.Y) - (p2.Y - p1.Y) * (p4.X - p3.X);

        /// <summary>
        /// Длина вектора.
        /// </summary>
        public static Func<Vertex, Vertex, float> VectorLength = (p1, p2) =>
                           (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        
        /// <summary>
        /// Добавление соседнего треугольниа.
        /// </summary>
        /// <param name="t">Треугольник.</param>
        public void SetTriangle(Triangle t)
        {
            if (!adjacentTriangles.Contains(t))
                adjacentTriangles.Add(t);
        }

        /// <summary>
        /// Удаление соседнего треугольника.
        /// </summary>
        /// <param name="t">Треугольник.</param>
        public void DelTriangle(Triangle t)
        {
            adjacentTriangles.Remove(t);
        }

        /// <summary>
        /// Унарный оператор равенства двух точек.
        /// </summary>
        /// <param name="a">Точка А.</param>
        /// <param name="b">Точка B.</param>
        /// <returns>True - точки равны, false - точки не равны.</returns>
        public static bool operator ==(Vertex a, Vertex b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Унарный оператор не равенства двух точек.
        /// </summary>
        /// <param name="a">Точка А.</param>
        /// <param name="b">Точка B.</param>
        /// <returns>True - точки не равны, false - точки равны.</returns>
        public static bool operator !=(Vertex a, Vertex b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Равенство двух точек.
        /// </summary>
        /// <param name="p">Точка для сравнения.</param>
        /// <returns>True - точки равны, false - точки не равны.</returns>
        public bool Equals(Vertex p)
        {
            return X == p.X && Y == p.Y;
        }

        /// <summary>
        /// Переопределение равенства двух объектов.
        /// </summary>
        public override bool Equals(Object obj)
        {
            return Equals(obj as Vertex);
        }

        /// <summary>
        /// Переопределение GetHashCode.
        /// </summary>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Метод, вызываемый при сериализации.
        /// </summary>
        /// <param name="info">Данные.</param>
        /// <param name="context">Источник.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", this.X);
            info.AddValue("Y", this.Y);
        }
    }
}