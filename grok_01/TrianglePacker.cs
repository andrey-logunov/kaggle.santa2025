using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace kaggle.santa2025.grok_01
{
    public class TrianglePacker
    {
        private int N;
        private readonly Random rnd = new Random(42);
        private List<Triangle> triangles = new();

        private double ComputeOverlapEnergy()
        {
            double e = 0.0;
            for (int i = 0; i < N; i++)
                for (int j = i + 1; j < N; j++)
                {
                    var a = triangles[i].GetVertices();
                    var b = triangles[j].GetVertices();
                    e += TriangleTriangleOverlap(a, b);
                }
            return e;
        }

        private double ComputeBoundaryEnergy(double S)
        {
            double p = 0.0;
            foreach (var t in triangles)
                foreach (var v in t.GetVertices())
                {
                    if (v.X < 0) p += -v.X;
                    if (v.X > S) p += v.X - S;
                    if (v.Y < 0) p += -v.Y;
                    if (v.Y > S) p += v.Y - S;
                }
            return p * 0.15; // slightly stronger than before
        }

        public double Energy(double S) => ComputeOverlapEnergy() + ComputeBoundaryEnergy(S);

        // Very fast and robust analytic overlap for two triangles
        private static double TriangleTriangleOverlap(Point2D[] A, Point2D[] B)
        {
            double area = 0.0;
            var polys = new[] { A, B };

            for (int p = 0; p < 2; p++)
            {
                var subject = polys[p];
                var clip = polys[1 - p];

                for (int i = 0; i < 3; i++)
                {
                    var p1 = subject[i];
                    var p2 = subject[(i + 1) % 3];
                    var output = new List<Point2D>();

                    for (int j = 0; j < 3; j++)
                    {
                        var cp1 = clip[j];
                        var cp2 = clip[(j + 1) % 3];

                        double d1 = (cp1.X - p1.X) * (p2.Y - p1.Y) - (cp1.Y - p1.Y) * (p2.X - p1.X);
                        double d2 = (cp2.X - p1.X) * (p2.Y - p1.Y) - (cp2.Y - p1.Y) * (p2.X - p1.X);

                        if (d2 >= -1e-12) output.Add(cp2);

                        if ((d1 > 0 && d2 <= 0) || (d1 <= 0 && d2 > 0))
                        {
                            double t = d1 / (d1 - d2);
                            output.Add(new Point2D(cp1.X + t * (cp2.X - cp1.X), cp1.Y + t * (cp2.Y - cp1.Y)));
                        }
                    }

                    if (output.Count >= 3)
                    {
                        var first = output[0];
                        for (int k = 1; k < output.Count - 1; k++)
                            area += Math.Abs((output[k].X - first.X) * (output[k + 1].Y - first.Y) -
                                            (output[k + 1].X - first.X) * (output[k].Y - first.Y));
                    }
                }
            }
            return area * 0.25;
        }

        private void RandomizeConfiguration(double S)
        {
            triangles.Clear();
            for (int i = 0; i < N; i++)
            {
                double x = 0.1 * S + rnd.NextDouble() * 0.8 * S;
                double y = 0.1 * S + rnd.NextDouble() * 0.8 * S;
                double a = rnd.NextDouble() * 2 * Math.PI;
                triangles.Add(new Triangle(new Point2D(x, y), a));
            }
        }

        private void CenterConfiguration(double S)
        {
            double cx = 0, cy = 0;
            foreach (var t in triangles) { cx += t.Position.X; cy += t.Position.Y; }
            cx /= N; cy /= N;
            double shiftX = S / 2 - cx;
            double shiftY = S / 2 - cy;
            foreach (var t in triangles)
            {
                t.Position = new Point2D(t.Position.X + shiftX, t.Position.Y + shiftY);
            }
        }

        private bool SimulatedAnnealingStep(double S, double T)
        {
            int i = rnd.Next(N);
            var t = triangles[i];
            var oldPos = t.Position;
            var oldAng = t.Angle;

            double maxStep = 0.25 * S;               // ← fixed maximum step
            double step = maxStep * Math.Min(1.0, T);

            if (rnd.NextDouble() < 0.7)
            {
                t.Position = new Point2D(
                    oldPos.X + (rnd.NextDouble() - 0.5) * step,
                    oldPos.Y + (rnd.NextDouble() - 0.5) * step);
            }
            else
            {
                t.Angle += (rnd.NextDouble() - 0.5) * step;
            }

            double oldE = Energy(S);
            double newE = Energy(S);

            if (newE > oldE && rnd.NextDouble() > Math.Exp(-(newE - oldE) / T))
            {
                t.Position = oldPos;
                t.Angle = oldAng;
            }

            return newE < 1e-9;
        }

        public (double side, List<(Point2D pos, double angle)> layout) Pack(int N)
        {
            this.N = N;
            double area = N * Math.Sqrt(3) / 4.0;
            double low = Math.Sqrt(area);
            double high = low * 1.8;

            double bestSide = high;
            List<(Point2D, double)> bestLayout = null;

            for (int iter = 0; iter < 80; iter++)
            {
                double mid = (low + high) / 2;

                RandomizeConfiguration(mid);

                double T = 1.2;
                for (int step = 0; step < 800_000; step++)
                {
                    SimulatedAnnealingStep(mid, T);
                    T *= 0.99992;
                    if (T < 1e-8) break;
                }

                CenterConfiguration(mid);   // ← crucial fix

                double finalE = Energy(mid);

                if (finalE < 1e-7)
                {
                    high = mid;
                    bestSide = mid;
                    bestLayout = triangles.Select(t => (t.Position, t.Angle)).ToList();
                }
                else
                {
                    low = mid;
                }

                if (high - low < 1e-6) break;
            }

            // Final polish
            RandomizeConfiguration(bestSide);
            double T0 = 1.0;
            for (int i = 0; i < 1_000_000; i++)
            {
                SimulatedAnnealingStep(bestSide, T0);
                T0 *= 0.99995;
            }
            CenterConfiguration(bestSide);

            return (bestSide, triangles.Select(t => (t.Position, t.Angle)).ToList());
        }

        public static void ExportToSvg(double side, List<(Point2D pos, double angle)> layout, string file = null)
        {
            if (file == null) file = $"packing_N{layout.Count}.svg";
            using var sw = new StreamWriter(file);
            sw.WriteLine($"<svg viewBox=\"0 0 {side} {side}\" xmlns=\"http://www.w3.org/2000/svg\">");
            sw.WriteLine($"  <rect width=\"{side}\" height=\"{side}\" fill=\"none\" stroke=\"black\" stroke-width=\"0.02\"/>");
            foreach (var (pos, a) in layout)
            {
                var t = new Triangle(pos, a);
                var v = t.GetVertices();
                sw.WriteLine($"  <polygon points=\"{v[0].X},{v[0].Y} {v[1].X},{v[1].Y} {v[2].X},{v[2].Y}\" " +
                             "fill=\"#4488ff\" opacity=\"0.8\" stroke=\"black\"/>");
            }
            sw.WriteLine("</svg>");
        }
    }
}
