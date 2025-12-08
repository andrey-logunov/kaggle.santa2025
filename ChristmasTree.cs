using System.Linq;

namespace kaggle.santa2025
{
    internal class ChristmasTree
    {
        public static readonly double[][] ETALON_VERTICES = [
            [ 0.0, 0.8 ],

            [ 0.125, 0.5 ],
            [ 0.075, 0.5 ],
            [ 0.25, 0.25 ],
            [ 0.075, 0.25 ],
            [ 0.35, 0.0 ],
            [ 0.075, 0.0 ],
            [ 0.075, -0.2 ],

            [ -0.075, -0.2 ],
            [ -0.075, 0.0 ],
            [ -0.35, 0.0 ],
            [ -0.075, 0.25 ],
            [ -0.25, 0.25 ],
            [ -0.075, 0.5 ],
            [ -0.125, 0.5 ],
        ];

        public readonly double[][] Vertices;

        public double X0 { get; private set; }
        public double Y0 { get; private set; }
        public double Deg { get; private set; }

        public double MinX { get; private set; }
        public double MaxX { get; private set; }
        public double MinY { get; private set; }
        public double MaxY { get; private set; }

        public ChristmasTree() : this(0.0, 0.0, 0.0) { }

        public ChristmasTree(double x0, double y0, double deg)
        {
            X0 = x0;
            Y0 = y0;
            Deg = deg;

            MinX = X0;
            MaxX = X0;
            MinY = Y0;
            MaxY = Y0;

            Vertices = new double[ETALON_VERTICES.Length][];
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
