namespace Tomography.Matrix
{
    using System;
    
    ///<summary>
    /// Класс-решатель СЛАУ методом Гаусса.
    ///</summary>
    public class GaussSol
    {
        double[][] initial_a_matrix;
        double[][] a_matrix;  // Матрица A.
        double[] initial_b_vector;
        double[] b_vector;  // Вектор B.
        double eps;  // Порядок точности для сравнения вещественных чисел.
        int size;  // Размерность задачи.

        ///<summary>
        /// Искомый вектор неизвестных.
        ///</summary>
        public double[] XVector { get; private set; }

        ///<summary>
        /// Вектор невязки.
        ///</summary>
        public double[] UVector { get; private set; }


        ///<summary>
        /// Решение СЛАУ вида Аx = b методом Гаусса.
        ///</summary>
        ///<param name="a_matrix">Матрица А.</param>
        ///<param name="b_vector">Вектор B.</param>
        ///<param name="eps">Точность вычислений.</param>
        public void LinearSystem(double[][] a_matrix, double[] b_vector, double eps)
        {
            if (a_matrix == null || b_vector == null)
                throw new ArgumentNullException("Один из параметров равен null.");

            int b_length = b_vector.Length;
            int a_length = a_matrix.Length * a_matrix.Length;
            if (a_length != b_length * b_length)
                throw new ArgumentException(@"Количество строк и столбцов в матрице A должно совпадать с количеством элементров в векторе B.");

            this.initial_a_matrix = a_matrix;   // Запоминаем исходную матрицу.
            this.a_matrix = (double[][])a_matrix.Clone();  // С копией будет производится вычисления.
            this.initial_b_vector = b_vector;  // Запоминаем исходный вектор.
            this.b_vector = (double[])b_vector.Clone();  // С копией будет производится вычисления.
            this.XVector = new double[b_length];
            this.UVector = new double[b_length];
            this.size = b_length;
            this.eps = eps;

            GaussSolve();
        }

        /// <summary>
        /// Нахождение решения СЛАУ методом Гаусса.
        /// </summary>
        private void GaussSolve()
        {
            int[] index = InitIndex();
            GaussForwardStroke(index);
            GaussBackwardStroke(index);
            GaussDiscrepancy();
        }

        /// <summary>
        /// Инициализация массива индексов столбцов.
        /// </summary>
        private int[] InitIndex()
        {
            int[] index = new int[size];
            for (int i = 0; i < index.Length; ++i)
                index[i] = i;
            return index;
        }

        /// <summary>
        /// Прямой ход метода Гаусса.
        /// </summary>
        private void GaussForwardStroke(int[] index)
        {
            // Перемещаемся по каждой строке сверху вниз.
            for (int i = 0; i < size; ++i)
            {
                // 1) Выбор главного элемента.
                double r = FindR(i, index);

                // 2) Преобразование текущей строки матрицы A.
                for (int j = 0; j < size; ++j)
                    a_matrix[i][j] /= r;

                // 3) Преобразование i-го элемента вектора b.
                b_vector[i] /= r;

                // 4) Вычитание текущей строки из всех нижерасположенных строк.
                for (int k = i + 1; k < size; ++k)
                {
                    double p = a_matrix[k][index[i]];
                    for (int j = i; j < size; ++j)
                        a_matrix[k][index[j]] -= a_matrix[i][index[j]] * p;
                    b_vector[k] -= b_vector[i] * p;
                    a_matrix[k][index[i]] = 0.0;
                }
            }
        }

        /// <summary>
        /// Обратный ход метода Гаусса.
        /// </summary>
        private void GaussBackwardStroke(int[] index)
        {
            // Перемещаемся по каждой строке снизу вверх.
            for (int i = size - 1; i >= 0; --i)
            {
                // 1) Задаётся начальное значение элемента x.
                double x_i = b_vector[i];

                // 2) Корректировка этого значения.
                for (int j = i + 1; j < size; ++j)
                    x_i -= XVector[index[j]] * a_matrix[i][index[j]];
                XVector[index[i]] = x_i;
            }
        }

        /// <summary>
        /// Вычисление невязки решения.
        /// U = b - x * A
        /// x - решение уравнения, полученное методом Гаусса.
        /// </summary>
        private void GaussDiscrepancy()
        {
            for (int i = 0; i < size; ++i)
            {
                double actual_b_i = 0.0;   // Результат перемножения i-строки исходной матрицы на вектор x.

                for (int j = 0; j < size; ++j)
                    actual_b_i += initial_a_matrix[i][j] * XVector[j];
                // i-й элемент вектора невязки.
                UVector[i] = initial_b_vector[i] - actual_b_i;
            }
        }

        /// <summary>
        /// Поиск главного элемента в матрице.
        /// </summary>
        private double FindR(int row, int[] index)
        {
            int max_index = row;
            double max = a_matrix[row][index[max_index]];
            double max_abs = Math.Abs(max);

            for (int cur_index = row + 1; cur_index < size; ++cur_index)
            {
                double cur = a_matrix[row][index[cur_index]];
                double cur_abs = Math.Abs(cur);
                if (cur_abs > max_abs)
                {
                    max_index = cur_index;
                    max = cur;
                    max_abs = cur_abs;
                }
            }
            // Меняем местами индексы столбцов.
            int temp = index[row];
            index[row] = index[max_index];
            index[max_index] = temp;

            return max;
        }
    }
}