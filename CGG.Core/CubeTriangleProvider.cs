using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace CGG.Core
{
    public class CubeTriangleProvider : ITriangleProvider
    {
        private readonly Triangle[] triangles;

        public CubeTriangleProvider(Point3D center, float a)
        {
            var points = new Point3D[8];
            for (int i = 0; i < 8; ++i)
                points[i] = center + new Vector3D(
                    a * ((i & 1) == 0 ? 1 : -1),
                    a * ((i & 2) == 0 ? 1 : -1),
                    a * ((i & 4) == 0 ? 1 : -1));
            triangles = new []
            {
                new Triangle(points[0], points[1], points[2]),
                new Triangle(points[1], points[2], points[3]),

                new Triangle(points[4], points[5], points[6]),
                new Triangle(points[5], points[6], points[7]),

                new Triangle(points[0], points[6], points[2]),
                new Triangle(points[0], points[6], points[4]),

                new Triangle(points[0], points[5], points[4]),
                new Triangle(points[0], points[5], points[1]),

                new Triangle(points[1], points[7], points[3]),
                new Triangle(points[1], points[7], points[5]),

                new Triangle(points[2], points[7], points[3]),
                new Triangle(points[2], points[7], points[6]),
            };
        }

        public IEnumerable<Triangle> Produce()
        {
            return triangles;
        }
    }
}