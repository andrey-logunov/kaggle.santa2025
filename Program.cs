using kaggle.santa2025.ChristmasTrees;
using kaggle.santa2025.Packing;
using System;

namespace kaggle.santa2025
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started...");

            int N = 5;

            Placement placementInit = new Placement();
            PackingShop.GenerateInitial(N).ForEach(t => placementInit.Trees.Add(ChristmasTreeFactory.Create(t.pos, t.template)));
            string svgInit = placementInit.ExportSvg();
            System.IO.File.WriteAllText($@"c:\temp\santa2025_{DateTime.Now:yyyyMMdd_HHmmss}_{N}.svg", svgInit);


            Placement placement = PackingShop.FindBestPacking(N, 50);
            string svg = placement.ExportSvg();
            System.IO.File.WriteAllText($@"c:\temp\santa2025_{DateTime.Now:yyyyMMdd_HHmmss}_{N}.svg", svg);

            Console.WriteLine("Done...");
        }
    }
}
