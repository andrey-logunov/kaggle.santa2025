using System.Numerics;

namespace kaggle.santa2025.grok_02
{
    struct Point2D {
        public double X, Y;
        public Point2D(double x, double y) { X = x; Y = y; }
        
        public static Point2D operator +(Point2D a, Point2D b) => new Point2D(a.X + b.X, a.Y + b.Y);
        public static Point2D operator -(Point2D a, Point2D b) => new Point2D(a.X - b.X, a.Y - b.Y);
    }
}
