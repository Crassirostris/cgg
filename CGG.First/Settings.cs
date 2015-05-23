using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CGG.First
{
    public static class Settings
    {
        public static int MinGrids = 10;
        public static int MaxGrids = 100;

        public static float AxesWidth = 3.5f;
        public static float Epsilon = 1e-5f;

        public static readonly Color BackgroundColor = Color.White;
        public static Pen AxesPen = new Pen(Color.Black)
        {
            EndCap = LineCap.ArrowAnchor,
            Width = Settings.AxesWidth
        };
        public static Pen GridPen = new Pen(Color.Gray);
        public static Pen PlotPen = new Pen(Color.Red) { Width = 3 };
        public static Brush GridMarksBrush = new SolidBrush(Color.Black);

        public static Font GridMarksFont = new Font(new FontFamily("Segoe UI"), 10, FontStyle.Regular);

        public static Func<float, float, float, float, float> Function =
            (x, a, b, c) => (a * x) / (b + x) / (c - x) / (c - x);

        public static Font AxisLegendFont = new Font(new FontFamily("Segoe UI"), 15, FontStyle.Bold);
        public static Brush AxisLegendBrush = new SolidBrush(Color.Blue);
    }
}
