using System;
using System.Linq;

namespace kaggle.santa2025.grok_02
{
    public static class Runner
    {
        public static void Run()
        {
            var packer = new PolygonPacker(2);  // change N here
            var (side, layout) = packer.Pack();

            Console.WriteLine($"\nN = 20 × 15-gon -> best square side = {side:F6}");
            Console.WriteLine("First few pieces:");
            foreach (var (x, y, _, a) in layout.Take(5))
                Console.WriteLine($"  ({x:F4}, {y:F4})  rot {a:F1}°");

            PolygonPacker.ExportSvg(side, layout);
        }
    }
}
