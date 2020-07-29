namespace Tomography.FEM
{
    using Delaunay;

    /// <summary>
    /// Класс окружности.
    /// </summary>
    public class Circle
    {
        /// <summary>
        /// Центр окружности.
        /// </summary>
        public Vertex<FiniteElement> Centre { get; }

        /// <summary>
        /// Радиус окружности.
        /// </summary>
        public float R { get; }

        /// <summary>
        /// Электропроводность среды.
        /// </summary>
        public float Permeability { get; }

        /// <summary>
        /// Нормальная производная на границе.
        /// </summary>
        public float? dfdn { get; }

        /// <summary>
        /// Заряд на границе двух сред.
        /// </summary>
        public float? sigma { get; }

        /// <summary>
        /// Входное напряжение U1.
        /// </summary>
        public float? U1 { get; }

        /// <summary>
        /// Выходное напряжение U2.
        /// </summary>
        public float? U2 { get; }


        /// <summary>
        /// Конструктор внутренней фигуры.
        /// </summary>
        /// <param name="Centre">Центр окружности.</param>
        /// <param name="R">Радиус окружности.</param>
        /// <param name="Permeability">Электропроводность среды.</param>
        /// <param name="sigma">Заряд на границе двух сред.</param>
        public Circle(Vertex<FiniteElement> Centre, float R, float Permeability, float sigma)
        {
            this.Centre = Centre;
            this.R = R;
            this.Permeability = Permeability;
            this.sigma = sigma;
        }

        /// <summary>
        /// Конструктор внешней фигуры.
        /// </summary>
        /// <param name="Centre">Центр окружности.</param>
        /// <param name="R">Радиус окружности.</param>
        /// <param name="Permeability">Электропроводность среды.</param>
        /// <param name="dfdn">Нормальная производная на границе.</param>
        /// <param name="U1">Входное напряжение U1.</param>
        /// <param name="U2">Выходное напряжение U2.</param>
        public Circle(Vertex<FiniteElement> Centre, float R, float Permeability, float dfdn, float U1, float U2)
        {
            this.Centre = Centre;
            this.R = R;
            this.Permeability = Permeability;
            this.dfdn = dfdn;
            this.U1 = U1;
            this.U2 = U2;
        }
        
        /// <summary>
        /// Точка внутри окружности.
        /// </summary>
        /// <param name="p">Точка.</param>
        /// <returns>True - точка внутри окружности.</returns>
        public bool HittingTheCircleInside(Vertex<FiniteElement> p)
        {
            return Centre.VectorLength(p) < R;
        }
    }
}