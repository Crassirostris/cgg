using System;

namespace CGG.Core
{
    public static class MathHelper
    {
        public static float Clampf(this float value, float min, float max)
        {
            return Math.Max(Math.Min(max, value), min);
        }

        public static double Clampf(this double value, double min, double max)
        {
            return Math.Max(Math.Min(max, value), min);
        }
    }
}