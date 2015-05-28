using System;
using System.Windows.Media.Media3D;

namespace CGG.Core
{
    public static class Vector3DExtenstions
    {
        public static Vector3D CrossProduct(this Vector3D left, Vector3D right)
        {
            return new Vector3D(
                left.Y * right.Z - left.Z * right.Y,
                left.Z * right.X - left.X * right.Z,
                left.X * right.Y - left.Y * right.X);
        }

        public static float DotProduct(this Vector3D left, Vector3D right)
        {
            return (float)(left.X * right.X + left.Y * right.Y + left.Z * right.Z);
        }

        public static Vector3D RotateAround(this Vector3D vector, Vector3D axis, float angle)
        {
            var rotationMatrix = new Matrix3D(
                Math.Cos(angle) + axis.X * axis.X * (1 - Math.Cos(angle)),
                axis.X * axis.Y * (1 - Math.Cos(angle)) - axis.Z * Math.Sin(angle),
                axis.X * axis.Z * (1 - Math.Cos(angle)) + axis.Y * Math.Sin(angle),
                0,
                axis.Y * axis.X * (1 - Math.Cos(angle)) + axis.Z * Math.Sin(angle),
                Math.Cos(angle) + axis.Y * axis.Y * (1 - Math.Cos(angle)),
                axis.Y * axis.Z * (1 - Math.Cos(angle)) - axis.X * Math.Sin(angle),
                0,
                axis.Z * axis.X * (1 - Math.Cos(angle)) - axis.Y * Math.Sin(angle),
                axis.Y * axis.Z * (1 - Math.Cos(angle)) + axis.X * Math.Sin(angle),
                Math.Cos(angle) + axis.Z * axis.Z * (1 - Math.Cos(angle)),
                0, 0, 0, 0, 1);
            return vector * rotationMatrix;
        }
    }
}