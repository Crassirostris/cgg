using System;
using System.Drawing;
using CGG.Core;

namespace Second
{
    internal class PlotInfo
    {
        public PointF F1 { get; private set; }
        public PointF F2 { get; private set; }
        public int A { get; private set; }
        public float E { get; private set; }
        public float B { get; private set; }
        public PointF InitialPoint { get; private set; }
        public PointF Direction { get; set; }
        public PointF Normal { get; set; }

        public PlotInfo(PointF f1, PointF f2, int a)
        {
            F1 = f1;
            F2 = f2;
            A = a;
            InitializeValues();
        }

        private void InitializeValues()
        {
            E = F1.Substract(F2).Length() / 2 / A;
            B = (float)(A * Math.Sqrt(1 - E * E));
            var median = F1.Add(F2).Divide(2f);
            Direction = F1.Equals(F2) ? new PointF(1, 0) : F1.Substract(F2).Normalize();
            Normal = new PointF(Direction.Y, -Direction.X);
            InitialPoint = median.Add(Normal.Muliply(B)).NearestIntegerPoint();
        }
    }
}