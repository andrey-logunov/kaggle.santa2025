using System;

namespace kaggle.santa2025.grok_01
{
    static class Extensions
    {
        public static void IfTrue(this bool condition, Action action) { if (condition) action(); }
    }
}
