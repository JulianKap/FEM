namespace Tomography.Geometry
{
    using System;
    using System.Collections.Generic;
    using Delaunay;


    /// <summary>
    /// Класс окружности.
    /// </summary>
    [Serializable]
    public class Circle: Figure
    {
        /// <summary>
        /// Центр окружности.
        /// </summary>
        public Vertex Centre { get; }

        /// <summary>
        /// Радиус окружности.
        /// </summary>
        public float R { get; }
        
        
        /// <summary>
        /// Конструктор внутренней фигуры.
        /// </summary>
        /// <param name="Centre">Центр окружности.</param>
        /// <param name="R">Радиус окружности.</param>
        public Circle(Vertex Centre, float R)
        {
            this.Centre = Centre;
            this.R = R;
        }
        
        /// <summary>
        /// Точка внутри окружности.
        /// </summary>
        /// <param name="p">Точка.</param>
        /// <returns>True - точка внутри окружности.</returns>
        public override bool HittingTheInside(Vertex p)
        {
            return Vertex.VectorLength(Centre, p) < R;
        }

        /// <summary>
        /// Кольцо точек окружности для триангуляции.
        /// </summary>
        /// <param name="size">Размерность сетки (>0).</param>
        /// <param name="structRibs">Структурные ребра границы фигуры.</param>
        /// <returns>Список точек.</returns>
        public override List<Vertex> PointsBorder(float size, out List<Rib> structRibs)
        {
            var points = new List<Vertex>();

            var step = (float)(Math.Acos((2 * R * R - size * size) / (2 * R * R)) * 180 / Math.PI);
            float n = 0;

            while (n < 360)
            {
                points.Add(PointInRound(ref n));
                n += step;
            }
            
            structRibs = SetRibs(points);
            points.Insert(0, new Vertex(Centre.X, Centre.Y));  // Центр окружности.

            return points;
        }

        /// <summary>
        /// Кольцо точек окружность для триангуляции Делоне с датчиками.
        /// </summary>
        /// <param name="size">Размерность сетки (>0).</param>
        /// <param name="structRibs">Структурные ребра границы фигуры.</param>
        /// <param name="viewPoints">Список точек наблюдения.</param>
        /// <returns>Список точек.</returns>
        public override List<Vertex> PointsBorderWithElectrodes(float size, out List<Rib> structRibs, out List<Vertex> viewPoints)
        {
            if (!U1.HasValue || !U2.HasValue)
                throw new ArgumentException("No has voltage!");

            var points = new List<Vertex>();

            var step = (float)(Math.Acos((2 * R * R - size * size) / (2 * R * R)) * 180 / Math.PI);
            var stepElectrode = 22.5f; // 16 точек на окружности.
            var site = size / 10;  // Место в близи точек наблюдения.

            float n = 90;
            float nElectrode = 90;

            viewPoints = new List<Vertex>();

            while (n < 450)
            {
                n += step;
                bool add = false;

                if (n > nElectrode - site)
                {
                    n = nElectrode;
                    nElectrode += stepElectrode;

                    if (nElectrode > 450)
                        break;

                    add = true;
                }
                
                var point = PointInRound( ref n);
                points.Add(point);

                if (add)
                {
                    viewPoints.Add(point);
                    
                    if (n == 90)
                        point.Potential = U1.Value;
                    else if (n == 270)
                        point.Potential = U2.Value;
                }
            }
            
            structRibs = SetRibs(points);
            points.Insert(0, new Vertex(Centre.X, Centre.Y));  // Центр окружности.

            return points;
        }
        
        /// <summary>
        /// Описание фигуры.
        /// </summary>
        /// <returns>Текстовое предстваление фигуры.</returns>
        public override string ToString()
        {
            var str = " ОКРУЖНОСТЬ:\n";
            str += String.Format("  Центр  х={0}  у={1}\n", Centre.X, Centre.Y);
            str += "  Радиус  " + R + "\n";
            str += "  Электропроводность  " + Permeability + "\n";

            if (dfdn.HasValue)   str += "  Нормальная производная  " + dfdn.Value + "\n";
            if (U1.HasValue)   str += "  Входное напряжение  " + U1.Value + "\n";
            if (U2.HasValue)   str += "  Выходное напряжение  " + U2.Value + "\n";

            return str;
        }

        /// <summary>
        /// Точка на окружности.
        /// </summary>
        /// <param name="n">Шаг.</param>
        /// <returns>Точка.</returns>
        Vertex PointInRound(ref float n)
        {
            // Угол в радианах.
            var angle = n * Math.PI / 180;
            var x = Centre.X + R * Math.Cos(angle);
            var y = Centre.Y + R * Math.Sin(angle);

            return new Vertex((float)x, (float)y);
        }
    }
}