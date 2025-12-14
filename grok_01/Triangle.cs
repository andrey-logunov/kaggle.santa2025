using System;
using System.Linq;

namespace kaggle.santa2025.grok_01
{
    public class Triangle
    {
        private static readonly Point2D[] BaseVertices = new[]
        {
            new Point2D(0, 0),
            new Point2D(1, 0),
            new Point2D(0.5, Math.Sqrt(3)/2)
        };

        public Point2D Position;  // centroid
        public double Angle;

        public Triangle(Point2D pos, double angle = 0) { Position = pos; Angle = angle; }

        public Point2D[] GetVertices()
        {
            double c = Math.Cos(Angle);
            double s = Math.Sin(Angle);
            return BaseVertices.Select(v =>
            {
                double rx = v.X * c - v.Y * s;
                double ry = v.X * s + v.Y * c;
                return new Point2D(rx + Position.X, ry + Position.Y);
            }).ToArray();
        }
    }
}
