namespace Tomography.Delaunay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    /// <summary>
    /// Класс триангуляции Делоне с ограничениями.
    /// Итеративный алгоритм с динамическим кэшированием поиска.
    /// </summary>
    public sealed class Triangulation<T> : IEnumerable<T> where T : Triangle<T>,  new()
    {
        ICollection<T> triangles;  // Коллекция треугольников.
        Cache<T> cache;  // Динамический кэш.


        /// <summary>
        /// Констуктор.
        /// </summary>
        /// <param name="vertex">Массив точек области триангуляции.</param>
        /// <param name="structRibs">Список структурных ребер для триангуляции.</param>
        /// <param name="splitArea">Построение неструктурной сетки триангуляции.</param>
        /// <param name="border">Плотность сетки на границах (>0).</param>
        public Triangulation(List<Vertex<T>> vertex, HashSet<Rib<T>> structRibs = null, bool splitArea = false, int border = 0)
        {
            // Список точек без дубликатов.
            var points = vertex.Distinct();
            // Точки для суперструктуры.
            var pointStruct = PointsStructure(points);
            // Создание суперструктуры и добавление первых треугольников в коллекцию.
            triangles = Superstructure(pointStruct).ToList();
            // Создание минимального кэша.
            cache = new Cache<T>(pointStruct[0], pointStruct[3]);
            cache.CreateCaсhe(triangles.First(), triangles.Last());
            // Добавление точек в триангуляцию с дальнейшей проверкой.
            AddPoints(points);

            // Список треугольников суперструктуры.
            var delete = new List<T>();
            foreach (var p in pointStruct)
                foreach (var t in p.Triangles)
                    delete.Add(t);

            // Построение неструктурной сетки триангуляции.
            if (splitArea)
                SplitAreaTriangulation(delete, structRibs.First(), border);

            // Вставка структурных отрезков в триангуляцию.
            if (structRibs != null)
                RestrictedTriangulation(structRibs);

            // Удаление суперструктуры.
            DeleteTriangleStructure(delete);
        }
        
        #region Построение триангуляции.
        /// <summary>
        /// Добавление точек в триангуляцию.
        /// </summary>
        /// <param name="points">Список точек.</param>
        private void AddPoints(IEnumerable<Vertex<T>> points)
        {
            foreach (var p in points)
            {
                // Нахождение треугольника по точке из кэша.
                var cacheTriangle = cache.GetTriangle(p);
                var rib = cacheTriangle.GetRibDivide(p);

                while (rib != null)
                {
                    cacheTriangle = rib.GetTriangleNeighbor(cacheTriangle);
                    rib = cacheTriangle.GetRibDivide(p);
                }

                // Новые треугольники для проверки.
                var uncheckTriangles = new HashSet<T>(PointInTriangle(cacheTriangle, p));
                
                // Новые треугольники проходят проверку и при необходимости изменяются.
                while (uncheckTriangles.Count > 0)
                    CheckTriangle(uncheckTriangles);
            }
        }
        
        /// <summary>
        /// Добавление точки внутрь треугольника.
        /// Треугольник разделяется на 3 новых.
        /// </summary>
        /// <param name="T">Треугольник, в который попадает точка.</param>
        /// <param name="node">Точка.</param>
        /// <returns>Возвращаются новые треугольники.</returns>
        private T[] PointInTriangle(T T, Vertex<T> node)
        {
            // Точки.
            var A = T.Points[0];
            var B = T.Points[1];
            var C = T.Points[2];
            // Ребра.
            var AB = T.GetRib(A, B);
            var BC = T.GetRib(B, C);
            // Новые треугольники.
            var LT = new T();
            var RT = new T();
            // Новые ребра.
            var OA = new Rib<T>(node, A, LT, T);
            var OB = new Rib<T>(node, B, LT, RT);
            var OC = new Rib<T>(node, C, RT, T);

            AB.UpdateTriangle(T, LT);
            BC.UpdateTriangle(T, RT);

            T.UpdateRib(OA, AB);
            T.UpdateRib(OC, BC);

            LT.SetRibs(AB, OB, OA);
            RT.SetRibs(BC, OB, OC);

            // Добавление новых треугольников в коллекцию.
            triangles.Add(LT);
            triangles.Add(RT);
            
            node.SetTriangle(T, LT, RT);
            A.SetTriangle(LT);
            B.SetTriangle(LT, RT);
            B.DelTriangle(T);
            C.SetTriangle(RT);

            (new T()).Сlockwise(T, LT, RT);
            cache.UpdateCache(LT, RT);

            return new T[] { T, LT, RT };
        }

        /// <summary>
        /// Проверка треугольников на условие Делоне.
        /// При необходимости выполняется флип.
        /// </summary>
        /// <param name="uncheckedTriangles">Треугольники для проверки.</param>
        private void CheckTriangle(HashSet<T> uncheckedTriangles)
        {
            // Треугольник, не удовлетворяющий условию Делоне.
            T flipTriangle = null;

            var lastTriangle = uncheckedTriangles.Last();
            var check = true;
            
            // Проверка на условие Делоне.
            foreach (var rib in lastTriangle.Ribs)
            {
                if (rib == null)
                    continue;

                flipTriangle = rib.GetTriangleNeighbor(lastTriangle);

                // Если соседний треугольник ребра null, переход к другому ребру.
                if (flipTriangle == null)
                    continue;

                // Точки для проверки на условие Делоне.
                var p0 = flipTriangle.GetVertexOpposite(rib);
                var p1 = rib.A;
                var p2 = lastTriangle.GetVertexOpposite(rib);
                var p3 = rib.B;

                if (!MethModSumAngels(p1, p2, p3, p0))
                {
                    check = false;
                    break;
                } 
            }

            // Если check = true, треугольник удовлетворяет условию Делоне и в дальнейшей проверке не участвует.
            if (check)
            {
                uncheckedTriangles.Remove(lastTriangle);
                return;
            }
            
            Flip(lastTriangle, flipTriangle);
            // Добавление нового треугольника для проверки.
            uncheckedTriangles.Add(flipTriangle);       
        }
        
        /// <summary>
        /// Производится флип треугольников.
        /// </summary>
        /// <param name="T1">Первый треугольник.</param>
        /// <param name="T2">Второй треугольник.</param>
        private void Flip(T T1, T T2)
        {
            // Старое ребро.
            var oldRib = T2.GetRibNeighbor(T1);
            // Вершины нового ребра.
            var C = T1.GetVertexOpposite(oldRib);
            var D = T2.GetVertexOpposite(oldRib);
            // Новое ребро.
            var СD = new Rib<T>(C, D, T1, T2);
            // Ребра, для которых необходимо обновить треугольники.
            var BC = T1.GetRib(oldRib.B, C);
            var AD = T2.GetRib(oldRib.A, D);

            BC.UpdateTriangle(T1, T2);
            AD.UpdateTriangle(T2, T1);

            T1.UpdateRib(СD, oldRib);
            T2.UpdateRib(СD, oldRib);
            T1.UpdateRib(AD, BC);
            T2.UpdateRib(BC, AD);
            
            oldRib.A.DelTriangle(T2);
            oldRib.B.DelTriangle(T1);
            СD.A.SetTriangle(T2);
            СD.B.SetTriangle(T1);

            (new T()).Сlockwise(T1, T2);
        }

        /// <summary>
        /// Модифицированная проверка суммы противолежащих углов (Условия Делоне).
        /// Алгоритм описан в книге Триангуляция Делоне и её применение. – Томск: Изд-во Том. ун-та, 2002. – 128 с. 21 стр.
        /// </summary>
        /// <param name="a">Вершина A.</param>
        /// <param name="b">Вершина Б.</param>
        /// <param name="c">Вершина С.</param>
        /// <param name="p0">Точка для проверки.</param>
        /// <returns>True - выполняются условия Делоне, false - не выполняются.</returns>
        private bool MethModSumAngels(Vertex<T> a, Vertex<T> b, Vertex<T> c, Vertex<T> p0)
        {
            var sAlfa = (p0.X - a.X) * (p0.X - c.X) + (p0.Y - a.Y) * (p0.Y - c.Y);
            var sBeta = (b.X - a.X) * (b.X - c.X) + (b.Y - a.Y) * (b.Y - c.Y);

            if (sAlfa < 0 && sBeta < 0)
                return false;

            if (sAlfa >= 0 && sBeta >= 0)
                return true;

            var summ = Math.Abs(((p0.X - a.X) * (p0.Y - c.Y) - (p0.X - c.X) * (p0.Y - a.Y))) * sBeta +
                       Math.Abs(((b.X - a.X) * (b.Y - c.Y) - (b.X - c.X) * (b.Y - a.Y))) * sAlfa;

            if (summ >= 0)
                return true;

            return false;
        }
        #endregion

        #region Неструктурная сетка области триангуляции.
        /// <summary>
        /// Построение неструктурной сетки триангуляции.
        /// </summary>
        /// <param name="delete">Список треугольников суперструктуры.</param>
        /// <param name="rib">Средний размер ребер триангуляции.</param>
        /// <param name="border">Плотность сетки на границах (>0).</param>
        private void SplitAreaTriangulation(List<T> delete, Rib<T> rib, int border = 0)
        {
            // Максимальная площадь треугольников триангуляции.
            var maxSquare = (Math.Sqrt(3) / 4 * Math.Pow(rib.Lenght, 2)) * 1.5f;
            // Треугольники для разбиения.
            var trianglesDivide = triangles.Except(delete).Where(t2 => t2.square > maxSquare).OrderByDescending(rr => rr.square);
            
            int i = 0;

            while (trianglesDivide.Count() > 0)
            {
                var points = new HashSet<T>();
                // Точки треугольников, которые будут разбиваться.
                var pointsAddTriangles = new HashSet<Vertex<T>>();

                foreach (var t in trianglesDivide)
                {
                    if (points != null)
                    {
                        if (pointsAddTriangles.Contains(t.Points[0]))
                            continue;
                        else if (pointsAddTriangles.Contains(t.Points[1]))
                            continue;
                        else if (pointsAddTriangles.Contains(t.Points[2]))
                            continue;

                        points.Add(t);
                        pointsAddTriangles.Add(t.Points[0]);
                        pointsAddTriangles.Add(t.Points[1]);
                        pointsAddTriangles.Add(t.Points[2]);
                    }
                    else
                    {
                        points.Add(t);
                        pointsAddTriangles.Add(t.Points[0]);
                        pointsAddTriangles.Add(t.Points[1]);
                        pointsAddTriangles.Add(t.Points[2]);
                    }
                }

                if (border != 0)
                    if (i++ > border)
                        break;
                
                AddPoints(points.Select(p => p.centre));
            }
        }
        #endregion

        #region Триангуляция с ограничениями.
        /// <summary>
        /// Вставка структурных ребер в триангуляцию.
        /// </summary>
        /// <param name="structRibs">Список структурных ребер для триангуляции.</param>
        private void RestrictedTriangulation(HashSet<Rib<T>> structRibs)
        {
            while (structRibs.Count > 0)
            {
                var pointsNew = SearchPointsCross(structRibs);

                if (pointsNew == null)
                    continue;

                AddPoints(new HashSet<Vertex<T>> { pointsNew });
            }
        }

        /// <summary>
        /// Поиск точек пересечения для структурного ребра.
        /// </summary>
        /// <param name="structRibs">Структурные ребра.</param>
        /// <returns>Точка пересечения.</returns>
        private Vertex<T> SearchPointsCross(HashSet<Rib<T>> structRibs)
        {
            // Поиск ближайших ребер, которые могут пересекаться со структурным (последнее в списке).
            var checkRibs = SearchRibsCross(structRibs);

            if (checkRibs == null)
                return null;

            var structRib = structRibs.Last();
            var A = structRib.A;
            var B = structRib.B;

            Vertex<T> pointNew = null;

            bool del = false;

            // Поиск точек пересечения.
            foreach (var ri in checkRibs)
            {
                pointNew = Vertex<T>.PointCross(A, B, ri.A, ri.B);

                // Точка пересечения является одной из точек проверяемого ребра.
                if (pointNew == ri.A || pointNew == ri.B)
                {
                    del = true; // Доработать поиск точек пересечения!!
                    break;
                }

                if (pointNew != null)
                    break;
            }

            // Разбиение структурного ребра.
            var newRibA = new Rib<T>(pointNew, B, null, null);
            var newRibB = new Rib<T>(pointNew, A, null, null);

            newRibA.SetBorderValue(structRib);
            newRibB.SetBorderValue(structRib);
            
            structRibs.Add(newRibA);
            structRibs.Add(newRibB);

            structRibs.Remove(structRib);

            if (del)
                return null;
            
            return pointNew;
        }

        /// <summary>
        /// Поиск ребер, пересекающихся со структурным ребром.
        /// </summary>
        /// <param name="structRibs">Структурные ребра.</param>
        /// <returns>Ребра, пересекающие структурное.</returns>
        private HashSet<Rib<T>> SearchRibsCross(HashSet<Rib<T>> structRibs)
        {
            var checkRibs = new HashSet<Rib<T>>();
            
            var structRib = structRibs.Last();
            var A = structRib.A;
            var B = structRib.B;

            var tria = new List<T>();
            tria.AddRange(A.Triangles);
            tria.AddRange(B.Triangles);

            foreach (var t in tria.Distinct())
            {
                // Поиск ребра, равного структурному.
                for (int i = 0; i < 3; i++)
                    if (t.Ribs[i].A == A && t.Ribs[i].B == B || t.Ribs[i].A == B && t.Ribs[i].B == A)
                    {
                        t.Ribs[i].SetBorderValue(structRib);

                        structRibs.Remove(structRib);
                        return null;
                    }

                // Добавление ближайщих ребер в список для поиска точки пересечения.
                if (t.Points[0] == A)      checkRibs.Add(t.Ribs[1]);
                else if (t.Points[1] == A) checkRibs.Add(t.Ribs[2]);
                else if (t.Points[2] == A) checkRibs.Add(t.Ribs[0]);
            }
            
            return checkRibs;
        }
        #endregion

        #region Суперструктура.
        /// <summary>
        /// Создание суперструктуры (прямоугольник).
        /// </summary>
        /// <param name="points">Массив точек, охватывающих все точки триангуляции.</param>
        private  IEnumerable<T> Superstructure(Vertex<T>[] points)
        {
            // Прямоугольник разбивается на 2 треугольника.
            var leftTriangle = new T();
            var rightTriangle = new T();

            // Создание ребер для новых треугольников.
            var diagonal = new Rib<T>(points[0], points[3], leftTriangle, rightTriangle);
            var left = new Rib<T>(points[0], points[1], leftTriangle, null);
            var right = new Rib<T>(points[2], points[3], rightTriangle, null);
            var top = new Rib<T>(points[0], points[2], rightTriangle, null);
            var bottom = new Rib<T>(points[1], points[3], leftTriangle, null);
            
            // Добавление новых ребер в треугольники.
            leftTriangle.SetRibs(left, bottom, diagonal);
            rightTriangle.SetRibs(right, top, diagonal);
            
            points[0].SetTriangle(leftTriangle, rightTriangle);
            points[3].SetTriangle(leftTriangle, rightTriangle);
            points[1].SetTriangle(leftTriangle);
            points[2].SetTriangle(rightTriangle);

            (new T()).Сlockwise(leftTriangle, rightTriangle);

            return new T[] { leftTriangle, rightTriangle };
        }

        /// <summary>
        /// Нахождение точек для суперструктуры.
        /// Охватываются все точки триангуляции.
        /// </summary>
        /// <param name="points">Список точек триангуляции.</param>
        /// <returns>Возвращаются координаты прямоугольника, который является суперструктурой.</returns>
        private Vertex<T>[] PointsStructure(IEnumerable<Vertex<T>> points)
        {
            var p00 = new Vertex<T>(points.Min(p => p.X) - 1, points.Min(p => p.Y) - 1);
            var p11 = new Vertex<T>(points.Max(p => p.X) + 1, points.Max(p => p.Y) + 1);
            var p01 = new Vertex<T>(p00.X, p11.Y);
            var p10 = new Vertex<T>(p11.X, p00.Y);

            return new Vertex<T>[] { p00, p01, p10, p11 };
        }

        /// <summary>
        /// Удаление треугольников суперструктуры.
        /// </summary>
        /// <param name="delete">Удаление треугольников из коллекции.</param>
        private void DeleteTriangleStructure(List<T> delete)
        {
            foreach (var t in delete)
            {
                triangles.Remove(t);

                for (int i = 0; i < 3; i++)
                    t.Points[i].DelTriangle(t);
            }
        }
        #endregion

        #region IEnumerable.
        /// <summary>
        /// Реализация интерфейса IEnumerable.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return triangles.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}