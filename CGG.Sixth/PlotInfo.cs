using System;
using System.Windows.Media.Media3D;
using CGG.Core;

namespace CGG.Sixth
{
    internal class PlotInfo
    {
        public ITriangleProvider TriangleProvider { get; private set; }
        public Point3D ViewPoint { get; private set; }
        public float ViewDistance { get; private set; }
        public Vector3D Forward { get; private set; }
        public Vector3D Right { get; private set; }
        public Vector3D Up { get { return Forward.CrossProduct(Right); } }

        public Vector3D GlobalUp { get; private set; }

        public PlotInfo(ITriangleProvider triangleProvider, Point3D viewPoint, float viewDistance, Vector3D forward, Vector3D right)
        {
            TriangleProvider = triangleProvider;
            ViewPoint = viewPoint;
            ViewDistance = viewDistance;
            forward.Normalize();
            Forward = forward;
            right.Normalize();
            Right = right;
            GlobalUp = new Vector3D(0, 0, 1);
        }

        public void Turn(TurnDirection turnDirection)
        {
            switch (turnDirection)
            {
                case TurnDirection.Left:
                    Turn(GlobalUp, Settings.TurnAngle);
                    break;
                case TurnDirection.Right:
                    Turn(GlobalUp, -Settings.TurnAngle);
                    break;
                case TurnDirection.Up:
                    Turn(Right, -Settings.TurnAngle);
                    break;
                case TurnDirection.Down:
                    Turn(Right, Settings.TurnAngle);
                    break;
            }
        }

        private void Turn(Vector3D rotationAxis, float angle)
        {
            Forward = Forward.RotateAround(rotationAxis, angle);
            Right = Right.RotateAround(rotationAxis, angle);
            ViewPoint = (Point3D) ((Vector3D) ViewPoint).RotateAround(rotationAxis, angle);
        }
    }
}