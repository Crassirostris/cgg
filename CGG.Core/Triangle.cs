using System;
using System.Drawing;
using System.Windows.Media.Media3D;

namespace CGG.Core
{
    public class Triangle
    {
        private Vector3D normal;

        public Point3D P1 { get; private set; }
        public Point3D P2 { get; private set; }
        public Point3D P3 { get; private set; }
        public Vector3D Normal { get { return normal; } }
        public Point3D Center { get { return (Point3D) (((Vector3D)P1 + (Vector3D)P2 + (Vector3D)P3) / 3); } }

        public Triangle(Point3D p1, Point3D p2, Point3D p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            normal = (P2 - P1).CrossProduct(P3 - P1);
            normal.Normalize();
        }

        public float DistanceFrom(Point3D point)
        {
            var d1 = Normal.CrossProduct(P2 - P1).DotProduct(point - P1);
            var d2 = Normal.CrossProduct(P3 - P2).DotProduct(point - P2);
            var d3 = Normal.CrossProduct(P1 - P3).DotProduct(point - P3);
            if (d1 * d2 > -float.Epsilon && d2 * d3 > -float.Epsilon && d1 * d3 >= -float.Epsilon)
                return Math.Abs(Normal.DotProduct(point - P1));
            var t1 = ((point - P1).DotProduct(P2 - P1) / (P2 - P1).LengthSquared).Clampf(0, 1);
            var distance1 = ((P1 - point) + t1 * (P2 - P1)).Length;
            var t2 = ((point - P2).DotProduct(P3 - P2) / (P3 - P2).LengthSquared).Clampf(0, 1);
            var distance2 = ((P2 - point) + t2 * (P3 - P2)).Length;
            var t3 = ((point - P3).DotProduct(P1 - P3) / (P1 - P3).LengthSquared).Clampf(0, 1);
            var distance3 = ((P3 - point) + t3 * (P1 - P3)).Length;
            return (float) Math.Min(distance1, Math.Min(distance2, distance3));
        }

        public PointF[] ProjectOn(Point3D viewPoint, float viewDistance, Vector3D front, Vector3D left, Vector3D up)
        {
            return new[]
            {
                ProjectOn(P1 - viewPoint, viewDistance, front, left, up), 
                ProjectOn(P2 - viewPoint, viewDistance, front, left, up), 
                ProjectOn(P3 - viewPoint, viewDistance, front, left, up), 
            };
        }

        private PointF ProjectOn(Vector3D a, float viewDistance, Vector3D front, Vector3D left, Vector3D up)
        {
            var d = a * viewDistance / a.DotProduct(front) - front * viewDistance;
            return new PointF(-d.DotProduct(left), d.DotProduct(up));
        }

        public float MaxDistanceTo(Point3D point)
        {
            return (float) Math.Max((point - P1).Length, Math.Max((point - P2).Length, (point - P3).Length));
        }
    }
}