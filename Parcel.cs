using System.Text;

namespace kaggle.santa2025
{
    internal class Parcel
    {
        const decimal MIN_XY = -100.0m;
        const decimal MAX_XY = 100.0m;

        readonly ChristmasTree[] trees;

        public Parcel(int numTrees)
        {
            trees = new ChristmasTree[numTrees];
            for (int i = 0; i < numTrees; i++)
            {
                //trees[i] = new ChristmasTree(MIN_XY + i * 0.7 + 1.0, 0.0, 0.0);
                trees[i] = new ChristmasTree(MIN_XY + (decimal)i * (0.35m + 0.1m) + 1.0m, i % 2 == 0 ? 0.0m : 0.25m, i % 2 == 0 ? 0.0m : 180.0m);
            }
        }

        public StringBuilder Submit(StringBuilder sb, bool raw = false)
        {
            for (int i = 0; i < trees.Length; i++)
            {
                string s = raw
                    ? $"{trees.Length:D3}_{i}: {trees[i]}"
                    : $"{trees.Length:D3}_{i},s{trees[i].X0:0.0#########},s{trees[i].Y0:0.0#########},s{trees[i].Deg:0.0#########}";
                sb.AppendLine(s);
            }
            return sb;
        }
    }
}
