namespace Tomography.Delaunay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Geometry;

    /// <summary>
    /// Класс триангуляции Делоне с ограничениями.
    /// Итеративный алгоритм с динамическим кэшированием поиска.
    /// </summary>
    [Serializable]
    public sealed class Triangulation : IDisposable
    {
        [NonSerialized]
        Cache cache;  // Динамический кэш.

        /// <summary>
        /// Коллекция треугольников.
        /// </summary>
        public ICollection<Triangle> triangles { get; private set; }


        /// <summary>
        /// Констуктор.
        /// </summary>
        /// <param name="vertex">Массив точек области триангуляции.</param>
        /// <param name="structRibs">Список структурных ребер для триангуляции.</param>
        /// <param name="figures">Список фигур.</param>
        public Triangulation(List<Vertex> vertex, List<Rib> structRibs = null, List<IFigure> figures = null)
        {
            // Список точек без дубликатов.
            var points = vertex.Distinct();
            // Точки для суперструктуры.
            var pointStruct = PointsStructure(points);
            // Создание суперструктуры.
            Superstructure(pointStruct);
            // Создание минимального кэша.
            cache = new Cache(pointStruct[0], pointStruct[3]);
            cache.CreateCaсhe(triangles.First(), triangles.Last());
            // Добавление точек в триангуляцию с дальнейшей проверкой.
            AddPoints(points);

            if (figures != null)
                // Построение неструктурной сетки триангуляции.
                SplitAreaTriangulation(figures, structRibs.First());
            else
                // Удаление суперструктуры.
                DeleteSuperstructure(pointStruct);

            // Вставка структурных отрезков в триангуляцию.
            if (structRibs != null)
                RestrictedTriangulation(structRibs);

            cache.Dispose();
        }
        
        #region Построение триангуляции.
        /// <summary>
        /// Добавление точек в триангуляцию.
        /// </summary>
        /// <param name="points">Список точек.</param>
        void AddPoints(IEnumerable<Vertex> points)
        {
            foreach (var p in points)
                AddPoint(p);
        }

        /// <summary>
        /// Добавление точки в триангуляцию.
        /// </summary>
        /// <param name="point">Точка.</param>
        void AddPoint(Vertex point)
        {
            // Нахождение треугольника по точке из кэша.
            var cacheTriangle = cache.GetTriangle(point);
            var rib = cacheTriangle.GetRibDivide(point);

            while (rib != null)
            {
                cacheTriangle = rib.GetTriangleNeighbor(cacheTriangle);
                rib = cacheTriangle.GetRibDivide(point);
            }

            // Новые треугольники для проверки.
            var uncheckTriangles = new HashSet<Triangle>(PointInTriangle(cacheTriangle, point));

            // Новые треугольники проходят проверку и при необходимости изменяются.
            while (uncheckTriangles.Count != 0)
                CheckTriangle(uncheckTriangles);
        }

        /// <summary>
        /// Добавление точки внутрь треугольника.
        /// Треугольник разделяется на 3 новых.
        /// </summary>
        /// <param name="T">Треугольник, в который попадает точка.</param>
        /// <param name="node">Точка.</param>
        /// <returns>Возвращаются новые треугольники.</returns>
        Triangle[] PointInTriangle(Triangle T, Vertex node)
        {
            // Точки.
            var A = T.Points[0];
            var B = T.Points[1];
            var C = T.Points[2];
            // Ребра.
            var AB = T.GetRib(A, B);
            var BC = T.GetRib(B, C);
            // Новые треугольники.
            var LT = new Triangle();
            var RT = new Triangle();
            // Новые ребра.
            var OA = new Rib(node, A, LT, T);
            var OB = new Rib(node, B, LT, RT);
            var OC = new Rib(node, C, RT, T);

            AB.UpdateTriangle(T, LT);
            BC.UpdateTriangle(T, RT);

            T.UpdateRib(OA, AB);
            T.UpdateRib(OC, BC);

            LT.SetRibs(AB, OB, OA);
            RT.SetRibs(BC, OB, OC);

            // Задание значения проводимости новых треугольников.
            if (T.Permeability.HasValue)
            {
                LT.Permeability = T.Permeability.Value;
                RT.Permeability = T.Permeability.Value;
            }

            // Добавление новых треугольников в коллекцию.
            triangles.Add(LT);
            triangles.Add(RT);

            // Соседние треугольники точек.
            node.SetTriangle(T);
            node.SetTriangle(LT);
            node.SetTriangle(RT);
            A.SetTriangle(LT);
            B.SetTriangle(LT);
            B.SetTriangle(RT);
            B.DelTriangle(T);
            C.SetTriangle(RT);

            cache.UpdateCache(LT);
            cache.UpdateCache(RT);

            return new Triangle[] { T, LT, RT };
        }

        /// <summary>
        /// Проверка треугольников на условие Делоне.
        /// При необходимости выполняется флип.
        /// </summary>
        /// <param name="uncheckedTriangles">Треугольники для проверки.</param>
        void CheckTriangle(HashSet<Triangle> uncheckedTriangles)
        {
            // Треугольник, не удовлетворяющий условию Делоне.
            Triangle flipTriangle = null;

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
        void Flip(Triangle T1, Triangle T2)
        {
            // Старое ребро.
            var oldRib = T2.GetRibNeighbor(T1);
            // Вершины нового ребра.
            var C = T1.GetVertexOpposite(oldRib);
            var D = T2.GetVertexOpposite(oldRib);
            // Новое ребро.
            var СD = new Rib(C, D, T1, T2);
            // Ребра, для которых необходимо обновить треугольники.
            var BC = T1.GetRib(oldRib.B, C);
            var AD = T2.GetRib(oldRib.A, D);

            BC.UpdateTriangle(T1, T2);
            AD.UpdateTriangle(T2, T1);

            T1.UpdateRib(СD, oldRib);
            T2.UpdateRib(СD, oldRib);
            T1.UpdateRib(AD, BC);
            T2.UpdateRib(BC, AD);

            // Соседние треугольники точек.
            oldRib.A.DelTriangle(T2);
            oldRib.B.DelTriangle(T1);
            СD.A.SetTriangle(T2);
            СD.B.SetTriangle(T1);
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
        bool MethModSumAngels(Vertex a, Vertex b, Vertex c, Vertex p0)
        {
            var c1 = p0.X - a.X;
            var c2 = p0.Y - a.Y;
            var c3 = b.X - a.X;
            var c4 = b.X - c.X;
            var c5 = b.Y - a.Y;
            var c6 = b.Y - c.Y;
            var c7 = p0.X - c.X;
            var c8 = p0.Y - c.Y;
            
            var sAlfa = c1 * c7 + c2 * c8;
            var sBeta = c3 * c4 + c5 * c6;
            
            if (sAlfa < 0 && sBeta < 0)
                return false;

            if (sAlfa >= 0 && sBeta >= 0)
                return true;

            var summ = Math.Abs((c1 * c8 - c7 * c2)) * sBeta + Math.Abs((c3 * c6 - c4 * c5)) * sAlfa;

            return summ >= 0 ? true : false;
        }
        #endregion

        #region Неструктурная сетка области триангуляции.
        /// <summary>
        /// Построение неструктурной сетки триангуляции.
        /// </summary>
        /// <param name="figures">Список фигур.</param>
        /// <param name="rib">Средний размер ребер триангуляции.</param>
        void SplitAreaTriangulation(List<IFigure> figures, Rib rib)
        {
            DeleteSuperstructureFigures(figures);

            // Максимальная площадь треугольников триангуляции.
            var maxSquare = (Math.Sqrt(3) / 4 * Math.Pow(rib.lenght, 2)) * 1.5f;
            // Треугольники для разбиения.
            var trianglesDivide = triangles.Where(t2 => t2.square > maxSquare).OrderByDescending(rr => rr.square);

            while (trianglesDivide.Any())
            {
                var points = new List<Vertex>();
                // Точки треугольников, которые будут разбиваться.
                var pointsAddTriangles = new HashSet<Vertex>();

                foreach (var t in trianglesDivide)
                {
                    if (pointsAddTriangles.Contains(t.Points[0]) || pointsAddTriangles.Contains(t.Points[1]) || pointsAddTriangles.Contains(t.Points[2]))
                        continue;

                    points.Add(Triangle.centre(t));
                    pointsAddTriangles.Add(t.Points[0]);
                    pointsAddTriangles.Add(t.Points[1]);
                    pointsAddTriangles.Add(t.Points[2]);
                }

                AddPoints(points);
            }
        }
        #endregion

        #region Триангуляция с ограничениями.
        /// <summary>
        /// Вставка структурных ребер в триангуляцию.
        /// </summary>
        /// <param name="structRibs">Список структурных ребер для триангуляции.</param>
        void RestrictedTriangulation(List<Rib> structRibs)
        {
            while (structRibs.Count != 0)
            {
                // Поиск ближайших ребер, которые могут пересекаться со структурным (последнее в списке).
                var checkRibs = SearchRibsCross(structRibs);

                if (checkRibs == null)  continue;

                var structRib = structRibs.Last();
                var A = structRib.A;
                var B = structRib.B;

                Vertex pointNew = null;

                bool del = false;

                // Поиск точек пересечения.
                foreach (var ri in checkRibs)
                {
                    pointNew = Vertex.PointCross(A, B, ri.A, ri.B);

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
                var newRibA = new Rib(pointNew, B, null, null);
                var newRibB = new Rib(pointNew, A, null, null);

                newRibA.SetBorderValue(structRib);
                newRibB.SetBorderValue(structRib);

                structRibs.Add(newRibA);
                structRibs.Add(newRibB);

                structRibs.Remove(structRib);

                if (del)   continue;

                AddPoint(pointNew);
            }
        }
        
        /// <summary>
        /// Поиск ребер, пересекающихся со структурным ребром.
        /// </summary>
        /// <param name="structRibs">Структурные ребра.</param>
        /// <returns>Ребра, пересекающие структурное.</returns>
        HashSet<Rib> SearchRibsCross(List<Rib> structRibs)
        {
            var checkRibs = new HashSet<Rib>();
            
            var structRib = structRibs.Last();
            var A = structRib.A;
            var B = structRib.B;
            
            foreach (var t in A.adjacentTriangles)
            {
                // Поиск ребра, равного структурному.
                for (int i = 0; i < 3; i++)
                    if (t.Ribs[i].A == A && t.Ribs[i].B == B || t.Ribs[i].A == B && t.Ribs[i].B == A)
                    {
                        t.Ribs[i].SetBorderValue(structRib);
                        t.Ribs[i].hasStruct = true;

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
        void Superstructure(Vertex[] points)
        {
            // Прямоугольник разбивается на 2 треугольника.
            var leftTriangle = new Triangle();
            var rightTriangle = new Triangle();

            // Создание ребер для новых треугольников.
            var diagonal = new Rib(points[0], points[3], leftTriangle, rightTriangle);
            var left = new Rib(points[0], points[1], leftTriangle, null);
            var right = new Rib(points[2], points[3], rightTriangle, null);
            var top = new Rib(points[0], points[2], rightTriangle, null);
            var bottom = new Rib(points[1], points[3], leftTriangle, null);
            
            // Добавление новых ребер в треугольники.
            leftTriangle.SetRibs(left, bottom, diagonal);
            rightTriangle.SetRibs(right, top, diagonal);

            // Соседние треугольники точек.
            points[0].SetTriangle(leftTriangle);
            points[0].SetTriangle(rightTriangle);
            points[3].SetTriangle(leftTriangle);
            points[3].SetTriangle(rightTriangle);
            points[1].SetTriangle(leftTriangle);
            points[2].SetTriangle(rightTriangle);

            triangles = new List<Triangle> { leftTriangle, rightTriangle };
        }

        /// <summary>
        /// Нахождение точек для суперструктуры.
        /// Охватываются все точки триангуляции.
        /// </summary>
        /// <param name="points">Список точек триангуляции.</param>
        /// <returns>Возвращаются координаты прямоугольника, который является суперструктурой.</returns>
        Vertex[] PointsStructure(IEnumerable<Vertex> points)
        {
            var p00 = new Vertex(points.Min(p => p.X) - 1, points.Min(p => p.Y) - 1);
            var p11 = new Vertex(points.Max(p => p.X) + 1, points.Max(p => p.Y) + 1);
            var p01 = new Vertex(p00.X, p11.Y);
            var p10 = new Vertex(p11.X, p00.Y);

            return new Vertex[] { p00, p01, p10, p11 };
        }

        /// <summary>
        /// Удаление треугольников суперструктуры.
        /// </summary>
        /// <param name="pointStruct">Удаление треугольников из коллекции.</param>
        void DeleteSuperstructure(Vertex[] pointStruct)
        {
            var delete = new HashSet<Triangle>();

            for (byte i = 0; i < 4; i++)
                foreach (var t in pointStruct[i].adjacentTriangles)
                    delete.Add(t);

            foreach (var t in delete)
            {
                triangles.Remove(t);

                for (int i = 0; i < 3; i++)
                    t.Points[i].DelTriangle(t);
            }

            delete.Clear();
        }

        /// <summary>
        /// Удаление треугольник, не относящихся к фигуре.
        /// </summary>
        /// <param name="figures">Список фигур.</param>
        void DeleteSuperstructureFigures(List<IFigure> figures)
        {
            var delete = new List<Triangle>();
            foreach (var t in triangles)
            {
                var check = true;
                foreach (var f in figures)
                    if (f.HittingTheInside(Triangle.centre(t)))
                    {
                        t.Permeability = f.Permeability;
                        check = false;
                    }

                if (check)
                    delete.Add(t);
            }

            foreach (var t in delete)
            {
                triangles.Remove(t);

                for (byte i = 0; i < 3; i++)
                    t.Points[i].DelTriangle(t);
            }

            delete.Clear();
        }
        #endregion
        
        #region IDisposable
        /// <summary>
        /// Диструтор.
        /// </summary>
        public void Dispose()
        {
            if (triangles != null)
                triangles.Clear();
        }
        #endregion
    }
}