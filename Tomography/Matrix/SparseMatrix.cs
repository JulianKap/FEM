namespace Tomography.Matrix
{
    using System;

    /// <summary>
    /// Класс разреженной матрицы.
    /// </summary>
    public class SparseMatrix
    {
        /// <summary>
        /// Cодержит все не нулевые элементы матрицы.
        /// </summary>
        public double[] aelem { get; }

        /// <summary>
        /// Cодержит индексы столбцов с не нулевыми элементами матрицы.
        /// </summary>
        public int[] jptr { get; }

        /// <summary>
        /// Cодержит число элементов в каждой строке матрицы.
        /// </summary>
        public int[] iptr { get; }


        /// <summary>
        /// Конструктор.
        /// </summary>
        public SparseMatrix(double[] aelem, int[] jptr, int[] iptr)
        {
            this.aelem = aelem;
            this.jptr = jptr;
            this.iptr = iptr;
        }
        
        /// <summary>
        /// Преобразует в эквивалентное строковое представление с использованием указанного формата.
        /// </summary>
        /// <returns>Строковое представление.</returns>
        public override string ToString()
        {
            string str = "";

            for (int j = 0; j < aelem.Length; j++)
            {
                str += aelem[j].ToString();
                if (j != aelem.Length - 1) str += " ";
            }
            str += Environment.NewLine;

            for (int j = 0; j < jptr.Length; j++)
            {
                str += jptr[j].ToString();
                if (j != jptr.Length - 1) str += " ";
            }
            str += Environment.NewLine;

            for (int j = 0; j < iptr.Length; j++)
            {
                str += iptr[j].ToString();
                if (j != iptr.Length - 1) str += " ";
            }
            str += Environment.NewLine;

            return str;
        }
    }
}
