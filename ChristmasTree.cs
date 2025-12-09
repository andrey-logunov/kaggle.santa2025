using System.Linq;

namespace kaggle.santa2025
{
    internal class ChristmasTree
    {
        public static readonly decimal[][] ETALON_VERTICES = [
            [ 0.0m, 0.8m ],

            [ 0.125m, 0.5m ],
            [ 0.0625m, 0.5m ],
            [ 0.2m, 0.25m ],
            [ 0.1m, 0.25m ],
            [ 0.35m, 0.0m ],
            [ 0.075m, 0.0m ],
            [ 0.075m, -0.2m ],

            [ -0.075m, -0.2m ],
            [ -0.075m, 0.0m ],
            [ -0.35m, 0.0m ],
            [ -0.1m, 0.25m ],
            [ -0.2m, 0.25m ],
            [ -0.0625m, 0.5m ],
            [ -0.125m, 0.5m ],
        ];

        public readonly decimal[][] Vertices;

        public decimal X0 { get; private set; }
        public decimal Y0 { get; private set; }
        public decimal Deg { get; private set; }

        public decimal MinX { get; private set; }
        public decimal MaxX { get; private set; }
        public decimal MinY { get; private set; }
        public decimal MaxY { get; private set; }

        public ChristmasTree() : this(0.0m, 0.0m, 0.0m) { }

        public ChristmasTree(decimal x0, decimal y0, decimal deg)
        {
            X0 = x0;
            Y0 = y0;
            Deg = deg;

            MinX = X0;
            MaxX = X0;
            MinY = Y0;
            MaxY = Y0;

            Vertices = new decimal[ETALON_VERTICES.Length][];
            for (int i = 0; i < ETALON_VERTICES.Length; i++)
            {
                var (newX, newY) = Geometry2D.RotateAroundPoint(X0, Y0, ETALON_VERTICES[i][0] + X0, ETALON_VERTICES[i][1] + Y0, Deg);
                Vertices[i] = [ newX, newY ];
                if (newX < MinX) MinX = newX;
                if (newX > MaxX) MaxX = newX;
                if (newY < MinY) MinY = newY;
                if (newY > MaxY) MaxY = newY;
            }
        }

        public override string ToString()
        {
            return $"[{string.Join(",", Vertices.Select(p => $"[{p[0]},{p[1]}]").ToArray())}]";
        }
    }
}
