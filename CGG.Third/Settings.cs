using System.Drawing;

namespace CGG.Third
{
    public static class Settings
    {
        public static readonly Brush PointBrush = new SolidBrush(Color.Black);
        public static readonly Brush BackgroundBrush = new SolidBrush(Color.White);
        public static readonly Pen HullPen = new Pen(Color.Crimson) { Width = 3 };

        public static readonly int PointRadius = 5;
    }
}
