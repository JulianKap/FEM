namespace Tomography.FEM
{
    using System;
    using Delaunay;

    /// <summary>
    /// Класс конечного элемента (треугольника).
    /// </summary>
    public class FiniteElement: Triangle<FiniteElement>
    {
        /// <summary>
        /// Матрица базисных функций треугольника.
        /// a(i) b(i) c(i)
        /// a(j) b(j) c(j)
        /// a(m) b(m) c(m)
        /// </summary>
        public float[][] basic { get; private set; }

        /// <summary>
        /// Матрица коэффициентов вершин треугольника.
        /// beta(ii) beta(ji) beta(ki)
        /// beta(ij) beta(jj) beta(kj)
        /// beta(ik) beta(jk) beta(kk)
        /// </summary>
        public float[][] coefficients { get; private set; }

        /// <summary>
        /// Электропроводность элемента.
        /// </summary>
        public float Permeability { get; set; }

        /// <summary>
        /// Определитель матрицы треугольника.
        /// </summary>
        public float Determ
        {
            get
            {
                return determ;
            }
            private set
            {
                determ = value * 2;
            }
        }

        private float determ;

        //public double Ex { get; private set; }
        //public double Ey { get; private set; }


        /// <summary>
        /// Пустрой конструктор.
        /// </summary>
        public FiniteElement(): base()
        {  }

        /// <summary>
        /// Расчет матриц.
        /// </summary>
        public void Calculate()
        {
            Determ = square;
            BasicFunctionElement();
            VertexCoefficients();
        }
        
        /// <summary>
        /// Вычисление базисных функций треугольника.
        /// </summary>
        private void BasicFunctionElement()
        {
            basic = new float[3][];
            
            for (int n = 0; n < 3; n++)
            {
                var result = new float[3];

                int n1 = n + 1;
                int n2 = n + 2;

                if (n1 == 3)       n1 = 0;
                if (n2 == 3)       n2 = 0;
                else if (n2 == 4)  n2 = 1;

                result[0] = ( Points[n1].Y - Points[n2].Y ) / determ;  // Коэфициент при X.
                result[1] = ( Points[n2].X - Points[n1].X ) / determ;  // Коэфициент при Y.
                result[2] = ( Points[n1].X * Points[n2].Y - Points[n2].X * Points[n1].Y ) / determ;  // Свободный член.

                basic[n] = result;
            }
            
            //if (!СheckBasicFunc())
            //    throw new ArgumentOutOfRangeException(" Ошибка базисных ф-ций. ");
        }

        /// <summary>
        /// Вычисление матрица коэффициентов вершин треугольника.
        /// </summary>
        private void VertexCoefficients()
        {
            coefficients = new float[3][];

            for (int i = 0; i < 3; i++)
            {
                var result = new float[3];

                for (int j = 0; j < 3; j++)
                    result[j] = 1.5f * determ * (basic[j][0] * basic[i][0] + basic[j][1] * basic[i][1]);

                coefficients[i] = result;
            }
        }

        ///// <summary>
        ///// Вычисление напряженности электромагнитного поля треугольника.
        ///// E = (Ex , Ey)
        ///// </summary>
        //internal void ElectromagneticFieldStrength()
        //{
        //    Ex = - ( Points[0].Potential.Value * basic[0][0] + Points[1].Potential.Value * basic[1][0] + Points[2].Potential.Value * basic[2][0] );
        //    Ey = - ( Points[0].Potential.Value * basic[0][1] + Points[1].Potential.Value * basic[1][1] + Points[2].Potential.Value * basic[2][1] );
        //}

        ///// <summary>
        ///// Проверка базисных функции треугольника.
        ///// </summary>
        ///// <returns>True - удовлетворяет, false - не удовлетворяет.</returns>
        //private bool СheckBasicFunc()
        //{
        //    if ((basic[0][0] + basic[1][0] + basic[2][0]) != 0f)
        //        return false;
        //    if ((basic[0][1] + basic[1][1] + basic[2][1]) != 0f)
        //        return false;
        //    if ((basic[0][2] + basic[1][2] + basic[2][2]) != 1f)
        //        return false;

        //    return true;
        //}

        /// <summary>
        /// Возвращает номер вершины треугольника.
        /// </summary>
        /// <param name="p">Вершина</param>
        /// <returns>Возвращается индекс вершины в треугольнике.</returns>
        public int GetVertexIndex(Vertex<FiniteElement> p)
        {
            for (int i = 0; i < 3; i++)
                if (p == Points[i])
                    return i;

            throw new ArgumentOutOfRangeException();
        }
    }
}