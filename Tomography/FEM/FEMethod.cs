namespace Tomography.FEM
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Delaunay;
    using Extreme.Mathematics;
    using Extreme.Mathematics.LinearAlgebra;
    using Extreme.Mathematics.LinearAlgebra.IterativeSolvers;
    using Extreme.Mathematics.LinearAlgebra.IterativeSolvers.Preconditioners;


    /// <summary>
    /// Класс метода конечных элементов.
    /// </summary>
    public class FEMethod
    {
        SparseMatrix<double> matrixA;  // Разреженная матрица А СЛАУ.
        SparseVector<double> vectorB;  // Разреженный вектор B СЛАУ.
        Vertex[] nullPoints;  // Точки с неизместными потенциалами.
        int n;  // Размерность СЛАУ.
        
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
        public FEMethod(IEnumerable<Triangle> elements)
        {
            // Узлы с неизвестными потенциалами.
            nullPoints = elements.SelectMany(p => p.Points).Distinct().Where(p => p.Potential == null).ToArray();

            // Идентификаторы узлов с неизвестными потенциалами.
            foreach (var p in nullPoints)
                p.ID = n++;

            // Расчет базисных функций и матрицы коеффициентов.
            foreach (var elem in elements)
                elem.BasicFunctionElement();

            // Построение разреженной СЛАУ.
            timeFill = Stopwatch.StartNew();
            FillSparseMatrix();
            timeFill.Stop();
            
            // Решение разреженной СЛАУ.
            timeSol = Stopwatch.StartNew();
            var conjugate = new BiConjugateGradientSolver<double>(matrixA);
            conjugate.Preconditioner = new IncompleteLUPreconditioner<double>(matrixA);
            var sol = conjugate.Solve(vectorB);
            timeSol.Stop();

            SetPotential(sol);

            matrixA.Dispose();
            vectorB.Dispose();
            sol.Dispose();
        }

        /// <summary>
        /// Формирование СЛАУ в разреженном виде.
        /// </summary>
        void FillSparseMatrix()
        {
            //matrixA = Matrix.CreateSparse<double>(n, n);
            vectorB = Vector.CreateSparse<double>(n);

            var values = new List<double>();
            var rows = new List<int>();
            var col = new List<int>();

            // Перебор точек с неизвестным потенциалом.
            for (int j = 0; j < n; j++)
            {
                var p0 = nullPoints[j];

                var vec = Vector.CreateSparse<double>(n);
                double sum = 0;

                foreach (var elem in p0.adjacentTriangles)
                {
                    // Индекс первой точки P0 в рассматриваемом треугольнике.
                    var i = elem.GetIndexVertex(p0);

                    // Перебор точек треугольника.
                    for (int k = 0; k < 3; k++)
                    {
                        // Вторая точка P1.
                        var p1 = elem.Points[k];

                        var res = elem.Permeability.Value * elem.GetCoefficients(i, k);

                        if (!p1.Potential.HasValue)
                            vec[p1.ID] += res;
                        else
                            sum -= p1.Potential.Value * res;

                        var rib = elem.Ribs[k];
                        if (rib.dfdn.HasValue)
                            sum += 0.5f * elem.Permeability.Value * rib.dfdn.Value * rib.lenght;
                    }
                }

                foreach (var v in vec.NonzeroElements)
                {
                    values.Add(v.Value);
                    rows.Add(v.Index);
                    col.Add(j);
                }

                vectorB[j] = sum;
            }

            matrixA = Matrix.CreateSparse(n, n, col.ToArray(), rows.ToArray(), values.ToArray());

            values.Clear();
            rows.Clear();
            col.Clear();
        }

        /// <summary>
        /// Добавление в узлы вычисленных потенциалов.
        /// </summary>
        /// <param name="pot">Массив значений потенциалов.</param>
        void SetPotential(DenseVector<double> pot)
        {
            for (int i = 0; i < n; i++)
                nullPoints[i].Potential = pot[i];
        }
    }
}