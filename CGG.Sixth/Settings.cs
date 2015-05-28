using System.Drawing;

namespace CGG.Sixth
{
    internal class Settings
    {
        public static float TurnAngle = 0.1f;
        public static float ZoomCoefficient = 500f;
        public static float ZoomStep = 5f;
        public static int TickInterval = 100;

        public static readonly Brush BackgroundBrush = new SolidBrush(Color.White);
        public static readonly Color ShapeColor = Color.Chartreuse;
        public static readonly Pen EdgeColor = new Pen(Color.Black) { Width = 2 };
    }
}