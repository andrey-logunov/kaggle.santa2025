using System.Text;

namespace kaggle.santa2025
{
    internal static class Solution
    {
        const int MAX_TREES = 200;

        public static string Simple(bool raw = false)
        {
            StringBuilder sb = new();
            sb.AppendLine("id,x,y,deg");
            for (int i = 1; i < MAX_TREES + 1; i++)
            {
                Parcel parcel = new(i);
                parcel.Submit(sb, raw);
            }
            return sb.ToString();
        }
    }
}
