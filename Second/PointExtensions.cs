using System;
using System.Drawing;
using System.Linq;

namespace Second
{
    public static class PointExtensions
    {
        private static readonly Point[] deltas = new []
        {
            new Point(1, 1),
            new Point(1, 0),
            new Point(1, -1),
            new Point(0, -1),
            new Point(-1, -1),
            new Point(-1, 0),
            new Point(-1, 1),
            new Point(0, 1),
        };

        public static Point Add(this Point left, Point right)
        {
            return new Point(left.X + right.X, left.Y + right.Y);
        }

        public static Point Substract(this Point left, Point right)
        {
            return new Point(left.X - right.X, left.Y - right.Y);
        }

        public static PointF Muliply(this Point left, float a)
        {
            return new PointF(left.X * a, left.Y * a);
        }

        public static Point Muliply(this Point left, int a)
        {
            return new Point(left.X * a, left.Y * a);
        }

        public static PointF Divide(this Point left, float a)
        {
            return new PointF(left.X / a, left.Y / a);
        }

        public static float Length(this Point point)
        {
            return (float)Math.Sqrt(point.X * point.X + point.Y * point.Y);
        }

        public static int LengthSquared(this Point point)
        {
            return point.X * point.X + point.Y * point.Y;
        }

        public static PointF Normalize(this Point point)
        {
            return point.Divide(point.Length());
        }

        public static Point[] Neighbors(this Point point)
        {
            return deltas.Select(d => point.Add(d)).ToArray();
        }
    }
}