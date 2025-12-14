using System;

namespace kaggle.santa2025.grok_01
{
    public static class Runner
    {
        public static void Run()
        {
            var packer = new TrianglePacker();

            int[] tests = { 1, 2, 4, 6, 8, 10, 16, 20 };

            foreach (int n in tests)
            {
                var (side, layout) = packer.Pack(n);

                Console.WriteLine("\n" + new string('=', 70));
                Console.WriteLine($"N = {n,2}  →  Square side = {side:F8}");
                Console.WriteLine(new string('-', 70));

                for (int i = 0; i < layout.Count; i++)
                {
                    var (pos, angRad) = layout[i];
                    double deg = angRad * 180 / Math.PI;
                    var tri = new Triangle(pos, angRad);
                    var v = tri.GetVertices();

                    Console.WriteLine($"Triangle {i + 1,2}: centroid({pos.X:F4}, {pos.Y:F4})  rot {deg,6:F1}°");
                    Console.WriteLine($"   v1 ({v[0].X:F5}, {v[0].Y:F5})");
                    Console.WriteLine($"   v2 ({v[1].X:F5}, {v[1].Y:F5})");
                    Console.WriteLine($"   v3 ({v[2].X:F5}, {v[2].Y:F5})\n");
                }

                TrianglePacker.ExportToSvg(side, layout, $"N{n:D2}.svg");
            }

            Console.WriteLine("All done — check the generated SVG files!");
            Console.ReadKey();
        }
    }
}
