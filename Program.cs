using System;

namespace kaggle.santa2025
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started...");
            string submission = Solution.Simple();
            System.IO.File.WriteAllText($@"c:\temp\santa2025_{DateTime.Now:yyyyMMdd_HHmmss}.csv", submission);
            Console.WriteLine("Done...");
        }
    }
}
