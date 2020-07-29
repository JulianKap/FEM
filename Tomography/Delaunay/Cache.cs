﻿namespace Tomography.Delaunay
{
    using System;
    using System.Linq;

    /// <summary>
    /// Класс динамического кэша.
    /// </summary>
    public sealed class Cache <T> where T: Triangle<T>
    {

        float xMin, yMin, xMax, yMax;  // Координаты прямоугольника, охватывающих все точки триангуляции.
        float xSize, ySize;  // Размер точки в кэше (x, y).

        int gain = 20;  // Предел роста кэша (зависит от коэффициета и размерности), начальное значение 20.
        int m = 2;  // Минимальный размер кэша, в дальнейшем увеличиваеся в m*m.
        int point = 0;  // Кол-во точек, используемых для кэша.

        const int growth = 5;  // Коэффициент роста динамического кэша, по умолчанию индекс 5 (оптимально от 3 до 8).
        T[][] cache;  // Массив для кэширования треугольников.


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="Min">Левая нижняя точка.</param>
        /// <param name="Max">Правая верхняя точка.</param>
        public Cache(Vertex<T> Min, Vertex<T> Max)
        {
            xMin = Min.X;
            yMin = Min.Y;
            xMax = Max.X;
            yMax = Max.Y;
            
            UpdatePointSize();
        }

        /// <summary>
        /// Создание минимального кэша.
        /// </summary>
        /// <param name="t1">Левый треугольник.</param>
        /// <param name="t2">Правый треугольник.</param>
        public void CreateCaсhe(T t1, T t2)
        {
            cache = UpdateArraySize(m);

            cache[0][0] = t1;
            cache[0][1] = t1;
            cache[1][0] = t2;
            cache[1][1] = t2;
        }

        /// <summary>
        /// Добавление (обновление) новых треугольников в кэше.
        /// </summary>
        /// <param name="newTriangles">Новые треугольники.</param>
        public void UpdateCache(params T [] newTriangles)
        {
            point += 2;

            // Перезапись старого кэша в новый с увеличенным размером.
            if (point >= gain)
            {
                var sizes = m * 2;
                var newCache = UpdateArraySize(sizes);

                for (int i = 0; i < m; i++)
                    for (int j = 0; j < m; j++)
                    {
                        var I = i * 2;
                        var J = j * 2;
                        var value = cache[i][j];

                        newCache[I][J] = value;
                        newCache[I][J + 1] = value;
                        newCache[I + 1][J] = value;
                        newCache[I + 1][J + 1] = value;
                    }
                
                m = sizes;
                gain = growth * m * m;
                cache = newCache;

                UpdatePointSize();
            }

            // Добавление новых точек в кэш.
            foreach (var t in newTriangles)
            {
                var i = (int)Math.Floor((t.Points.Sum(p => p.Y) / 3 - yMin) / ySize);
                var j = (int)Math.Floor((t.Points.Sum(p => p.X) / 3 - xMin) / xSize);
                
                cache[i][j] = t;
            }
        }
        
        /// <summary>
        /// Нахождение треугольника из кэша по точке.
        /// </summary>
        /// <param name="point">Точка для поиска.</param>
        /// <returns>Возвращение треугольника из кэша.</returns>
        public T GetTriangle(Vertex<T> point)
        {
            return cache[(int)Math.Floor((point.Y - yMin) / ySize)][(int)Math.Floor((point.X - xMin) / xSize)];
        }

        /// <summary>
        /// Обновление размера массива кэша.
        /// </summary>
        /// <param name="count">Колличество элементов массива.</param>
        /// <returns>Возвращение нового массива.</returns>
        private T[][] UpdateArraySize(int count)
        {
            var newCache = new T[count][];

            for (int i = 0; i < count; ++i)
                newCache[i] = new T[count];
            
            return newCache;
        }
        
        /// <summary>
        /// Обновление размера ячейки в кэше.
        /// </summary>
        private void UpdatePointSize()
        {
            xSize = (xMax - xMin) / m;
            ySize = (yMax - yMin) / m;
        }
    }
}