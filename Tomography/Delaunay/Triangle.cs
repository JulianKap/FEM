namespace Tomography.Delaunay
{
    using System;

    /// <summary>
    /// Класс треугольников.
    /// </summary>
    public class Triangle<T> where T : Triangle<T>
    {
        /// <summary>
        /// Массив ребер в треугольнике.
        /// </summary>
        public Rib<T>[] Ribs { get; protected set; } = new Rib<T>[3];

        /// <summary>
        /// Массив точек в треугольнике.
        /// </summary>
        public Vertex<T>[] Points { get; protected set; } = new Vertex<T>[3];

        /// <summary>
        /// Площадь треугольника.
        /// </summary>
        public float square { get; protected set; }

        /// <summary>
        /// Центр тяжести треугольника.
        /// </summary>
        public Vertex<T> centre { get; protected set; }


        /// <summary>
        /// Добавление ребер для нового треугольника.
        /// </summary>
        /// <param name="R1">Первое ребро.</param>
        /// <param name="R2">Второе ребро.</param>
        /// <param name="R3">Третье ребро.</param>
        public void SetRibs(Rib<T> R1, Rib<T> R2, Rib<T> R3)
        {
            Ribs[0] = R1;
            Ribs[1] = R2;
            Ribs[2] = R3;
            
            SquareTriangle();
        }

        /// <summary>
        /// Обновление ребра в треугольнике.
        /// </summary>
        /// <param name="newRib">Новое ребро.</param>
        /// <param name="oldRib">Старое ребро.</param>
        public void UpdateRib(Rib<T> newRib, Rib<T> oldRib)
        {
            if (Ribs[0] == oldRib)
                Ribs[0] = newRib;
            else if (Ribs[1] == oldRib)
                Ribs[1] = newRib;
            else
                Ribs[2] = newRib;
            
            SquareTriangle();
        }

        /// <summary>
        /// Нахождение ребра по двум точкам.
        /// </summary>
        /// <param name="A">Точка А.</param>
        /// <param name="B">Точка B.</param>
        /// <returns>Возвращаем ребро</returns>
        public Rib<T> GetRib(Vertex<T> A, Vertex<T> B)
        {
            foreach (var rib in Ribs)
            {
                if (rib.A == A && rib.B == B || rib.A == B && rib.B == A)
                    return rib;
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Нахождение ребра, принадлежащего двум треугольникам.
        /// </summary>
        /// <param name="T">Треугольник.</param>
        /// <returns>Возвращение соседнего ребра.</returns>
        public Rib<T> GetRibNeighbor(T T)
        {
            for (int i = 0; i < 3; i++)
            {
                var rib = Ribs[i];

                if (rib.T1 == T || rib.T2 == T)
                    return Ribs[i];
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Находится ребро, которое разделяет точку и любую вершину треугольника.
        /// </summary>
        /// <param name="p">Точка для проверки.</param>
        /// <returns>Если точка попадает в треугольник, возвращается null, иначе ребро.</returns>
        public Rib<T> GetRibDivide(Vertex<T> p)
        {
            foreach (var rib in Ribs)
            {
                // Если знак OA ^ OX совпадает со знаком OA ^ OY, то точки X и Y лежат на одной полуплоскости, иначе нет.
                // Используется формула косого произведения векторов.
                var y = GetVertexOpposite(rib);
                var oxSign = Math.Sign(Vertex<T>.Pseudoscalar(rib.A, rib.B, rib.A, p));
                var oySign = Math.Sign(Vertex<T>.Pseudoscalar(rib.A, rib.B, rib.A, y));

                if (oxSign == 0 || oySign == 0)
                    continue;

                if (oxSign != oySign)
                    return rib;
            }

            return null;
        }

        /// <summary>
        /// Нахождение вершины, противоположной ребру.
        /// </summary>
        /// <param name="rib">Ребро.</param>
        /// <returns>Возвращение вершины.</returns>
        public Vertex<T> GetVertexOpposite(Rib<T> rib)
        {
            if (Ribs[0] == rib)
                return Points[2];
            else if (Ribs[1] == rib)
                return Points[0];

            return Points[1];
        }
        
        /// <summary>
        /// Проверка равенства точки одной из вершин треугольника.
        /// </summary>
        /// <param name="p">Точка.</param>
        /// <returns>True - принадлежит, false - не принадлежит.</returns>
        public bool TrianglePointCheck(Vertex<T> p)
        {
            if (p == Points[0] || p == Points[1] || p == Points[2])
                return true;

            return false;
        }

        /// <summary>
        /// Вычисление площади треугольника.
        /// </summary>
        private void SquareTriangle()
        {
            // Периметр треугольника.
            var p = (Ribs[0].Lenght + Ribs[1].Lenght + Ribs[2].Lenght) / 2;
            square = (float)Math.Sqrt(p * (p - Ribs[0].Lenght) * (p - Ribs[1].Lenght) * (p - Ribs[2].Lenght));
        }

        /// <summary>
        /// Вычисление центра тяжести треугольника.
        /// </summary>
        private void CentreTriangle()
        {
            centre = new Vertex<T>((Points[0].X + Points[1].X + Points[2].X) / 3f, (Points[0].Y + Points[1].Y + Points[2].Y) / 3f);
        }

        /// <summary>
        /// Формирование структуры элемента, обход точек по часовой стрелке.
        /// </summary>
        /// <param name="triangles">Треугольники.</param>
        public void Сlockwise(params T[] triangles)
        {
            foreach (var t in triangles)
            {
                var A = t.Ribs[0].A;
                var B = t.Ribs[0].B;
                var C = new Vertex<T>();

                var rib1 = t.Ribs[1];

                Rib<T> a = null;
                Rib<T> b = null;
                
                if (rib1.A == A)
                {
                    a = t.Ribs[2];
                    b = rib1;
                    C = rib1.B;
                }
                else if (rib1.A == B)
                {
                    a = rib1;
                    b = t.Ribs[2];
                    C = rib1.B;
                }
                else if (rib1.B == A)
                {
                    a = t.Ribs[2];
                    b = rib1;
                    C = rib1.A;
                }
                else
                {
                    a = rib1;
                    b = t.Ribs[2];
                    C = rib1.A;
                }

                // Обновление вершин и ребер.
                t.Points[0] = A;
                t.Points[1] = B;
                t.Points[2] = C;

                t.Ribs[1] = a;
                t.Ribs[2] = b;

                t.CentreTriangle();
            }
        }
    }
}