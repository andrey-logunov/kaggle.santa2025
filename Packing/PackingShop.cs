using kaggle.santa2025.Geometry2D;
using System;
using System.Collections.Generic;

namespace kaggle.santa2025.Packing
{
    public static class PackingShop
    {
        public static List<(Vector2D pos, int template)> GenerateInitial(int numTrees, double spread = 10.0)
        {
            var list = new List<(Vector2D, int)>();
            Random random = new (2025);

            for (int i = 0; i < numTrees; i++)
            {
                double x = (random.NextDouble() - 0.5) * spread;
                double y = (random.NextDouble() - 0.5) * spread + i * 0.5; // Stagger vertically
                int rotation = random.Next(0, 360); // Or prefer base-down: 270 + rng.Next(-30,30)

                // Better: prefer base down (root pointing down)
                int baseDownIndex = 270; // Adjust based on your BASE_ROOT direction
                int rotationIndex = (baseDownIndex + random.Next(-45, 46)) % 360;

                list.Add((new Vector2D(x, y), rotationIndex));
            }
            return list;
        }

        public static Placement FindBestPacking(int numTrees, int numAttempts = 50)
        {
            Placement best = null;
            double bestSide = double.MaxValue;

            for (int attempt = 0; attempt < numAttempts; attempt++)
            {
                var initial = GenerateInitial(numTrees, spread: 15.0 + attempt * 2);
                var placement = GravityPacker.Pack(initial);

                if (placement.SideLength < bestSide - 1e-4)
                {
                    bestSide = placement.SideLength;
                    best = placement;
                    Console.WriteLine($"New best: {bestSide:F3} (attempt {attempt + 1})");
                }
            }

            return best;
        }
    }
}
