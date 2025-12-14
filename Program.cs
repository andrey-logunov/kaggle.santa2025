using kaggle.santa2025.ChristmasTrees;
using kaggle.santa2025.Geometry2D;
using kaggle.santa2025.Packing;
using System;

namespace kaggle.santa2025
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started...");

            int N = 3;

            Box box = new (N);
            ChristmasTree[] layout = box.Pack();
            double side = Box.Adjust(layout);
            string svg = Box.ExportSvg(side, layout);
            Console.WriteLine($"Side: {side:F4}");
            System.IO.File.WriteAllText($@"c:\temp\santa2025_{DateTime.Now:yyyyMMdd_HHmmss}_{N}.svg", svg);
            Console.WriteLine("Done...");
        }
    }
}
