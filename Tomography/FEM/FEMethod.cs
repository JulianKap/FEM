namespace Tomography.FEM
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using Delaunay;
    using Matrix;

    /// <summary>
    /// Класс метода конечных элементов.
    /// </summary>
    public class FEMethod
    {
        IEnumerable<FiniteElement> elements;  // Список элементов.
        List<Vertex<FiniteElement>> nullPoints;  // Узлы с неизвестными потенциалами.

        int n;  // Размерность СЛАУ.

        /// <summary>
        /// Разреженная матрица А СЛАУ.
        /// </summary>
        public SparseMatrix matrixA { get; private set; }

        /// <summary>
        /// Матрица А СЛАУ (не разреженная).
        /// </summary>
        public double[][] matrix { get; private set; }

        /// <summary>
        /// Вектор B СЛАУ.
        /// </summary>
        public double[] vectorB { get; private set; }

        /// <summary>
        /// Время построения СЛАУ.
        /// </summary>
        public Stopwatch timeFill { get; private set; }

        /// <summary>
        /// Время решения СЛАУ.
        /// </summary>
        public Stopwatch timeSol { get; private set; }


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="elements">Список элементов.</param>
        public FEMethod(IEnumerable<FiniteElement> elements)
        {
            this.elements = elements;
            // Узлы с неизвестными потенциалами.
            nullPoints = elements.SelectMany(p => p.Points).Distinct().Where(p => p.Potential == null).ToList();
            n = nullPoints.Count();

            // Идентификаторы узлов с неизвестными потенциалами.
            int i = 0;
            foreach (var p in nullPoints)
                p.ID = i++;

            // Расчет базисных функций и матрицы коеффициентов.
            foreach (var elem in elements)
                elem.Calculate();
            
            timeFill = Stopwatch.StartNew();

            //FillSparseMatrix();
            FillMatrix();

            timeFill.Stop();

            timeSol = Stopwatch.StartNew();

            GaussSol sol = new GaussSol();
            sol.LinearSystem(matrix, vectorB, 0.00001);

            timeSol.Stop();

            SetPotential(sol.XVector);
        }

        /// <summary>
        /// Формирование СЛАУ в обычном виде.
        /// </summary>
        private void FillMatrix()
        {
            matrix = new double[n][];
            vectorB = new double[n];

            int row = 0;  // Счетчик строк матрицы А.
            // Перебор точек с неизвестным потенциалом.
            foreach (var p0 in nullPoints)
            {
                var re = new double[n];
                double sum = 0;

                // Перебор треугольников, которым принадлежит вершина P0.
                foreach (var elem in p0.Triangles)
                {
                    // Индекс первой точки P0 в рассматриваемом треугольнике.
                    var i = elem.GetVertexIndex(p0);
                    // Перебор точек треугольника.
                    for (int k = 0; k < 3; k++)
                    {
                        // Вторая точка P1.
                        var p1 = elem.Points[k];

                        if (!p1.Potential.HasValue)
                            // Заполнение матрицы А.
                            re[p1.ID] += elem.Permeability * elem.coefficients[i][k];
                        else
                            sum -= p1.Potential.Value * elem.Permeability * elem.coefficients[i][k];

                        var rib = elem.Ribs[k];
                        if (rib.dfdn.HasValue && rib.dfdn.Value != 0)
                            sum += 0.5f * elem.Permeability * rib.dfdn.Value * rib.Lenght;
                        else if (rib.sigma.HasValue && rib.sigma.Value != 0)
                            sum += 0.5f * rib.sigma.Value * rib.Lenght;
                    }
                }
                vectorB[row] = sum;
                matrix[row] = re;
                row++;
            }
        }

        /// <summary>
        /// Формирование СЛАУ в разреженном виде.
        /// </summary>
        private void FillSparseMatrix()
        {
            // Разреженная матрица А.
            var aelemList = new List<double>();
            var jptrList = new List<int>();
            var iptr = new int[n + 1];

            // Вектор B.
            vectorB = new double[n];

            int row = 0;  // Счетчик строк матрицы А.
            int count = 1;  // Счетчик количества элементов в одной строке матрицы А.
            iptr[0] = count;  // Добавление первый индекса "1".
            
            // Перебор точек с неизвестным потенциалом.
            foreach (var p0 in nullPoints)
            {
                var re = new double[n];
                double sum = 0;

                foreach (var elem in p0.Triangles)
                {
                    // Индекс первой точки P0 в рассматриваемом треугольнике.
                    var i = elem.GetVertexIndex(p0);

                    // Перебор точек треугольника.
                    for (int k = 0; k < 3; k++)
                    {
                        // Вторая точка P1.
                        var p1 = elem.Points[k];

                        if (!p1.Potential.HasValue)
                            // Заполнение матрицы А.
                            re[p1.ID] += elem.Permeability * elem.coefficients[i][k];
                        else
                            sum -= p1.Potential.Value * elem.Permeability * elem.coefficients[i][k];

                        var rib = elem.Ribs[k];
                        if (rib.dfdn.HasValue && rib.dfdn.Value != 0)
                            sum += 0.5f * elem.Permeability * rib.dfdn.Value * rib.Lenght;
                        else if (rib.sigma.HasValue && rib.sigma.Value != 0)
                            sum += 0.5f * rib.sigma.Value * rib.Lenght;
                    }
                }

                for (int i = 0; i < n; i++)
                    if (re[i] != 0)
                    {
                        aelemList.Add(re[i]);
                        jptrList.Add(i);
                        count++;
                    }

                iptr[row + 1] = count;
                vectorB[row] = sum;
                row++;
            }

            matrixA = new SparseMatrix(aelemList.ToArray(), jptrList.ToArray(), iptr);
        }

        /// <summary>
        /// Добавление в узлы вычисленных потенциалов.
        /// </summary>
        /// <param name="pot">Массив значений потенциалов.</param>
        private void SetPotential(double[] pot)
        {
            for (int i = 0; i < n; i++)
                nullPoints[i].Potential = pot[i];
        }
    }
}