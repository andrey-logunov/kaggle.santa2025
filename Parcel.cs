using System.Text;

namespace kaggle.santa2025
{
    internal class Parcel
    {
        const double MIN_XY = -100.0;
        const double MAX_XY = 100.0;

        readonly ChristmasTree[] trees;

        public Parcel(int numTrees)
        {
            trees = new ChristmasTree[numTrees];
            for (int i = 0; i < numTrees; i++)
            {
                //trees[i] = new ChristmasTree(MIN_XY + i * 0.7 + 1.0, 0.0, 0.0);
                trees[i] = new ChristmasTree(MIN_XY + i * (0.425 + 0.010) + 1.0, i % 2 == 0 ? 0.0 : 0.25 - 0.002, i % 2 == 0 ? 0.0 : 180.0);
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
