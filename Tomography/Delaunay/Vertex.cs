namespace Tomography.Delaunay
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Класс вершин (точек в двумерном пространстве).
    /// </summary>
    public class Vertex<T> : IEquatable<Vertex<T>> where T: Triangle<T>
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
        /// Список треугольников, содержащих точку.
        /// </summary>
        public HashSet<T> Triangles { get; protected set; } = new HashSet<T>();

        /// <summary>
        /// Значение потенциала.
        /// </summary>
        public double? Potential { get; set; }

        /// <summary>
        /// Идентификатор точки.
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Пустой конструктор.
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
        }

        /// <summary>
        /// Добавление треугольника в список.
        /// </summary>
        /// <param name="triangles">Добавляемые треугольники.</param>
        public void SetTriangle(params T[] triangles)
        {
            foreach (var t in triangles)
                Triangles.Add(t);
        }

        /// <summary>
        /// Удаление треугольника из списка.
        /// </summary>
        /// <param name="t">Удаляемый треугольник. Если входной парамметр null, список полностью очищается.</param>
        public void DelTriangle(T t = null)
        {
            if (t != null)
                Triangles.Remove(t);
            else
                Triangles.Clear();
        }
        
        /// <summary>
        /// Нахождение точки пересечения двух векторов.
        /// </summary>
        /// <param name="p1">Координата 1.</param>
        /// <param name="p2">Координата 2.</param>
        /// <param name="p3">Координата 3.</param>
        /// <param name="p4">Координата 4.</param>
        /// <returns>Возвращается точка пересечения, или null в случае, если вектора не пересекаются.</returns>
        public static Vertex<T> PointCross(Vertex<T> p1, Vertex<T> p2, Vertex<T> p3, Vertex<T> p4)
        {
            var d = Pseudoscalar(p2, p1, p3, p4);
            var da = Pseudoscalar(p3, p1, p3, p4);
            var db = Pseudoscalar(p2, p1, p3, p1);

            var ta = da / d;
            var tb = db / d;

            if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
            {
                var dx = p1.X + ta * (p2.X - p1.X);
                var dy = p1.Y + ta * (p2.Y - p1.Y);
                
                return new Vertex<T>(dx, dy);
            }

            return null;
        }

        /// <summary>
        /// Псевдоскалярное векторное произведение.
        /// </summary>
        /// <param name="p1">Координата 1.</param>
        /// <param name="p2">Координата 2.</param>
        /// <param name="p3">Координата 3.</param>
        /// <param name="p4">Координата 4.</param>
        /// <returns>Возвращает псевдоскалярное векторное произведение.</returns>
        public static float Pseudoscalar(Vertex<T> p1, Vertex<T> p2, Vertex<T> p3, Vertex<T> p4)
        {
            return (p2.X - p1.X) * (p4.Y - p3.Y) - (p2.Y - p1.Y) * (p4.X - p3.X);
        }

        /// <summary>
        /// Длина вектора.
        /// </summary>
        /// <param name="p">Координата вектора.</param>
        /// <returns>Возвращается длина вектора.</returns>
        public float VectorLength(Vertex<T> p)
        {
            return (float)Math.Sqrt(Math.Pow(X - p.X, 2) + Math.Pow(Y - p.Y, 2));
        }
        
        /// <summary>
        /// Унарный оператор равенства двух точек.
        /// </summary>
        /// <param name="a">Точка А.</param>
        /// <param name="b">Точка B.</param>
        /// <returns>True - точки равны, false - точки не равны.</returns>
        public static bool operator ==(Vertex<T> a, Vertex<T> b)
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
        public static bool operator !=(Vertex<T> a, Vertex<T> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Равенство двух точек.
        /// </summary>
        /// <param name="p">Точка для сравнения.</param>
        /// <returns>True - точки равны, false - точки не равны.</returns>
        public bool Equals(Vertex<T> p)
        {
            return X == p.X && Y == p.Y;
        }

        /// <summary>
        /// Переопределение равенства двух объектов.
        /// </summary>
        public override bool Equals(Object obj)
        {
            return Equals(obj as Vertex<T>);
        }

        /// <summary>
        /// Переопределение GetHashCode.
        /// </summary>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}