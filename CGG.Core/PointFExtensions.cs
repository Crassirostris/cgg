using System;
using System.Drawing;
using System.Linq;

namespace CGG.Core
{
    public static class PointFExtensions
    {
        public static PointF Add(this PointF left, PointF right)
        {
            return new PointF(left.X + right.X, left.Y + right.Y);
        }

        public static PointF Substract(this PointF left, PointF right)
        {
            return new PointF(left.X - right.X, left.Y - right.Y);
        }

        public static PointF Muliply(this PointF left, float a)
        {
            return new PointF(left.X * a, left.Y * a);
        }

        public static PointF Divide(this PointF left, float a)
        {
            return new PointF(left.X / a, left.Y / a);
        }

        public static float Length(this PointF point)
        {
            return (float) Math.Sqrt(point.X * point.X + point.Y * point.Y);
        }

        public static Point NearestIntegerPoint(this PointF point)
        {
            var leftTopPoint = new Point((int)Math.Ceiling(point.X), (int)Math.Ceiling(point.Y));
            var candidates = new[]
            {
                leftTopPoint,
                new Point(leftTopPoint.X + 1, leftTopPoint.Y),
                new Point(leftTopPoint.X, leftTopPoint.Y + 1),
                new Point(leftTopPoint.X + 1, leftTopPoint.Y + 1)
            };
            return candidates
                .OrderBy(p => point.Substract(p).Length())
                .First();
        }

        public static PointF Normalize(this PointF point)
        {
            return point.Divide(point.Length());
        }

        public static float CrossProduct(this PointF left, PointF right)
        {
            return left.X * right.Y - left.Y * right.X;
        }
    }
}