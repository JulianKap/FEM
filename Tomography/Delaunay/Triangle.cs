namespace Tomography.Delaunay
{
    using System;

    /// <summary>
    /// Класс треугольников.
    /// </summary>
    [Serializable]
    public class Triangle
    {
        /// <summary>
        /// Массив ребер в треугольнике.
        /// </summary>
        public Rib[] Ribs { get; protected set; } = new Rib[3];

        /// <summary>
        /// Массив точек в треугольнике.
        /// </summary>
        public Vertex[] Points { get; protected set; } = new Vertex[3];

        /// <summary>
        /// Площадь треугольника.
        /// </summary>
        public float square { get; protected set; }
        
        #region Данные для МКЭ.
        /// <summary>
        /// Матрица базисных функций треугольника.
        /// a(i) b(i) c(i)
        /// a(j) b(j) c(j)
        /// a(m) b(m) c(m)
        /// </summary>
        public float[][] basic { get; private set; }
        
        /// <summary>
        /// Электропроводность элемента.
        /// </summary>
        public float? Permeability { get; set; }
        

        /// <summary>
        /// Вычисление матрица коэффициентов вершин треугольника.
        /// beta(ii) beta(ji) beta(ki)
        /// beta(ij) beta(jj) beta(kj)
        /// beta(ik) beta(jk) beta(kk)
        /// </summary>
        public float GetCoefficients(int i, int j)
        {
            return 3f * square * (basic[j][0] * basic[i][0] + basic[j][1] * basic[i][1]);
        }

        /// <summary>
        /// Вычисление базисных функций треугольника.
        /// </summary>
        public void BasicFunctionElement()
        {
            basic = new float[3][];

            for (byte n = 0; n < 3; n++)
            {
                var result = new float[3];

                int n1 = n + 1;
                int n2 = n + 2;

                if (n1 == 3) n1 = 0;
                if (n2 == 3) n2 = 0;
                else if (n2 == 4) n2 = 1;

                result[0] = (Points[n1].Y - Points[n2].Y) / (2 * square);  // Коэфициент при X.
                result[1] = (Points[n2].X - Points[n1].X) / (2 * square);  // Коэфициент при Y.
                result[2] = (Points[n1].X * Points[n2].Y - Points[n2].X * Points[n1].Y) / (2 * square);  // Свободный член.

                basic[n] = result;
            }
        }
        #endregion

        /// <summary>
        /// Добавление ребер для нового треугольника.
        /// </summary>
        /// <param name="R1">Первое ребро.</param>
        /// <param name="R2">Второе ребро.</param>
        /// <param name="R3">Третье ребро.</param>
        public void SetRibs(Rib R1, Rib R2, Rib R3)
        {
            Ribs[0] = R1;
            Ribs[1] = R2;
            Ribs[2] = R3;

            Сlockwise();
            SquareTriangle();
        }

        /// <summary>
        /// Обновление ребра в треугольнике.
        /// </summary>
        /// <param name="newRib">Новое ребро.</param>
        /// <param name="oldRib">Старое ребро.</param>
        public void UpdateRib(Rib newRib, Rib oldRib)
        {
            if (Ribs[0] == oldRib)
                Ribs[0] = newRib;
            else if (Ribs[1] == oldRib)
                Ribs[1] = newRib;
            else
                Ribs[2] = newRib;

            Сlockwise();
            SquareTriangle();
        }

        /// <summary>
        /// Нахождение ребра по двум точкам.
        /// </summary>
        /// <param name="A">Точка А.</param>
        /// <param name="B">Точка B.</param>
        /// <returns>Возвращаем ребро</returns>
        public Rib GetRib(Vertex A, Vertex B)
        {
            foreach (var rib in Ribs)
                if (rib.A == A && rib.B == B || rib.A == B && rib.B == A)
                    return rib;

            throw new ArgumentException();
        }

        /// <summary>
        /// Нахождение ребра, принадлежащего двум треугольникам.
        /// </summary>
        /// <param name="T">Треугольник.</param>
        /// <returns>Возвращение соседнего ребра.</returns>
        public Rib GetRibNeighbor(Triangle T)
        {
            for (byte i = 0; i < 3; i++)
            {
                var rib = Ribs[i];

                if (rib.T1 == T || rib.T2 == T)
                    return Ribs[i];
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Проверка принадлежности точки треугольнику.
        /// </summary>
        /// <param name="p">Точка для проверки.</param>
        /// <returns>Null - точка внутри, rib - ребро, соседнее с точкой.</returns>
        public Rib GetRibDivide(Vertex p)
        {
            foreach (var rib in Ribs)
            {
                // Если знак OA ^ OX совпадает со знаком OA ^ OY, то точки X и Y лежат на одной полуплоскости, иначе нет.
                // Используется формула косого произведения векторов.
                var y = GetVertexOpposite(rib);
                var oxSign = Math.Sign(Vertex.Pseudoscalar(rib.A, rib.B, rib.A, p));
                var oySign = Math.Sign(Vertex.Pseudoscalar(rib.A, rib.B, rib.A, y));

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
        public Vertex GetVertexOpposite(Rib rib)
        {
            if (Ribs[0] == rib)
                return Points[2];

            return Ribs[1] == rib? Points[0]: Points[1];
        }

        /// <summary>
        /// Возвращает номер вершины треугольника.
        /// </summary>
        /// <param name="p">Вершина</param>
        /// <returns>Возвращается индекс вершины в треугольнике.</returns>
        public int GetIndexVertex(Vertex p)
        {
            for (byte i = 0; i < 3; i++)
                if (p == Points[i])
                    return i;

            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Вычисление площади треугольника.
        /// </summary>
        void SquareTriangle()
        {
            // Периметр треугольника.
            var p = (Ribs[0].lenght + Ribs[1].lenght + Ribs[2].lenght) / 2;
            square = (float)Math.Sqrt(p * (p - Ribs[0].lenght) * (p - Ribs[1].lenght) * (p - Ribs[2].lenght));
        }
        
        /// <summary>
        /// Центр треугольника.
        /// </summary>
        public static Func<Triangle, Vertex> centre = (t) => 
                  new Vertex((t.Points[0].X + t.Points[1].X + t.Points[2].X) / 3f, 
                             (t.Points[0].Y + t.Points[1].Y + t.Points[2].Y) / 3f);

        /// <summary>
        /// Формирование структуры элемента, обход точек по часовой стрелке.
        /// </summary>
        void Сlockwise()
        {
            Vertex C = null;
            var A = Ribs[0].A;
            var B = Ribs[0].B;

            Rib a = null;
            Rib b = null;
            var rib1 = Ribs[1];

            if (rib1.A == A)
            {
                a = Ribs[2];
                b = rib1;
                C = rib1.B;
            }
            else if (rib1.A == B)
            {
                a = rib1;
                b = Ribs[2];
                C = rib1.B;
            }
            else if (rib1.B == A)
            {
                a = Ribs[2];
                b = rib1;
                C = rib1.A;
            }
            else
            {
                a = rib1;
                b = Ribs[2];
                C = rib1.A;
            }
            
            // Обновление вершин и ребер.
            Points[0] = A;
            Points[1] = B;
            Points[2] = C;

            Ribs[1] = a;
            Ribs[2] = b;
        }
    }
}