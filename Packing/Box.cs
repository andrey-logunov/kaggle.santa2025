using kaggle.santa2025.ChristmasTrees;
using kaggle.santa2025.CollisionDetection2D;
using kaggle.santa2025.Geometry2D;
using System;
using System.Linq;
using System.Text;

namespace kaggle.santa2025.Packing
{
    public class Box(int n)
    {
        public int N { get; set; } = n;

        readonly Random random = new(2025);
        ChristmasTree[] currentLayout;

        void Randomize(double side)
        {
            currentLayout = new ChristmasTree[N];
            for (int i = 0; i < N; i++)
            {
                currentLayout[i] = ChristmasTreeFactory.Create(new Vector2D(random.NextDouble() * side, random.NextDouble() * side), random.Next(ChristmasTreeFactory.NUM_OF_TEMPLATES));
            }
        }

        void Center(double side)
        {
            double centerX = 0, centerY = 0;
            foreach (var tree in currentLayout)
            {
                centerX += tree.Translation.X;
                centerY += tree.Translation.Y;
            }
            centerX /= N;
            centerY /= N;
            double deltaX = side / 2 - centerX;
            double deltaY = side / 2 - centerY;
            for (int i = 0; i < N; i++)
            {
                currentLayout[i] = ChristmasTreeFactory.Create(currentLayout[i].Translation.Translated(deltaX, deltaY), currentLayout[i].TemplateIndex);
            }
        }

        double GetOverlapEnergy()
        {
            double energy = 0.0;
            for (int i = 0; i < N - 1; i++)
                for (int j = i + 1; j < N; j++)
                {
                    CollisionResult result = ChristmasTreeFactory.DetectCollision(currentLayout[i], currentLayout[i]);
                    if (result.IsColliding)
                    {
                        energy += result.PenetrationDepth;
                    }
                }
            return energy;
        }

        double GetBoundaryEnergy(double side)
        {
            double penalty = 0.0;
            for (int i = 0; i < N; i++)
            {
                AABB box = ChristmasTreeFactory.GetAABB(currentLayout[i]);

                if (box.Min.X < 0) penalty += -box.Min.X;          // left of square
                if (box.Max.X > side) penalty += box.Max.X - side; // right of square
                if (box.Min.Y < 0) penalty += -box.Min.Y;          // bottom
                if (box.Max.Y > side) penalty += box.Max.Y - side; // top
            }
            return penalty * 0.1;                                  // very important coefficient!
        }

        double Energy(double side)
        {
            return GetOverlapEnergy() + GetBoundaryEnergy(side);
        }

        public ChristmasTree[] Pack()
        {
            double area = N * ChristmasTreeFactory.BASE_POLYGON_AREA;
            double low = Math.Sqrt(area) * 0.95;
            double high = low * 2.0;
            double bestSide = high;
            ChristmasTree[] bestLayout = null;

            for (int iteration = 0; iteration < 80; iteration++)
            {
                double mid = (low + high) / 2;
                Randomize(mid);

                double T = 2.0;
                for (int step = 0; step < 12_000; step++)
                {
                    int i = random.Next(N);
                    ChristmasTree oldTree = currentLayout[i];

                    // mutate
                    if (random.NextDouble() < 0.7)
                    {
                        currentLayout[i] = ChristmasTreeFactory.Create(
                            currentLayout[i].Translation.Translated((random.NextDouble() - 0.5) * 0.3 * Math.Min(1.0, T), (random.NextDouble() - 0.5) * 0.3 * Math.Min(1.0, T)),
                            currentLayout[i].TemplateIndex);
                    }
                    else
                    {
                        currentLayout[i] = ChristmasTreeFactory.Create(
                            currentLayout[i].Translation,
                            random.Next(ChristmasTreeFactory.NUM_OF_TEMPLATES));
                    }
                    double eNew = Energy(mid);
                    if (eNew > 0.1 && random.NextDouble() > Math.Exp(-eNew / T))
                    {
                        currentLayout[i] = oldTree; // reject
                    }

                    T *= 0.99995;
                }

                Center(mid);
                double e = Energy(mid);
                if (e < 0.1) // essentially perfect
                {
                    high = mid;
                    bestSide = mid;
                    bestLayout = currentLayout;
                }
                else low = mid;

                if (high - low < 1e-6) break;
            }

            // Final polish
            bestLayout ??= currentLayout;
            Center(bestSide);

            return bestLayout;
        }

        public static double Adjust(ChristmasTree[] layout)
        {
            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;
            for (int i = 0; i < layout.Length; i++)
            {
                AABB box = ChristmasTreeFactory.GetAABB(layout[i]);
                if (box.Min.X < minX) minX = box.Min.X;
                if (box.Max.X > maxX) maxX = box.Max.X;
                if (box.Min.Y < minY) minY = box.Min.Y;
                if (box.Max.Y > maxY) maxY = box.Max.Y;
            }

            double side = Math.Max(maxX - minX, maxY - minY);
            double deltaX = -minX;
            double deltaY = -minY;

            //double centerX = (maxX + minX) / 2;
            //double centerY = (maxY + minY) / 2;
            //double deltaX = side / 2 - centerX;
            //double deltaY = side / 2 - centerY;

            for (int i = 0; i < layout.Length; i++)
            {
                layout[i] = ChristmasTreeFactory.Create(layout[i].Translation.Translated(deltaX, deltaY), layout[i].TemplateIndex);
            }
            return side;
        }

        public static string ExportSvg(double side, ChristmasTree[] layout)
        {
            StringBuilder sb = new ();
            sb.AppendLine($"<svg viewBox=\"0 0 {side + 2} {side + 2}\" xmlns=\"http://www.w3.org/2000/svg\">");
            sb.AppendLine($"  <rect x=\"0\" y=\"0\" width=\"{side}\" height=\"{side}\" fill=\"none\" stroke=\"#333\" stroke-width=\"0.05\"/>");

            foreach (ChristmasTree tree in layout)
            {
                Polygon2D polygon = ChristmasTreeFactory.TEMPLATE_POLYGONS[tree.TemplateIndex].Translated(tree.Translation);
                string points = string.Join(" ", polygon.Vertices.Select(v => $"{v.X:F4},{v.Y:F4}"));
                sb.AppendLine($"  <polygon points=\"{points}\" fill=\"#4488ff\" opacity=\"0.7\" stroke=\"black\" stroke-width=\"0.02\"/>");
            }
            sb.AppendLine("</svg>");
            return sb.ToString();
        }
    }
}
