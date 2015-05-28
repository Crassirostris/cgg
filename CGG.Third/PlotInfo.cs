using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CGG.Core;

namespace CGG.Third
{
    internal class PlotInfo
    {
        private Size currentSize;

        public PlotInfo(Size currentSize)
        {
            this.currentSize = currentSize;
            Points = new List<PointF>();
        }

        public bool CanBuildHull { get { return Points.Count % 3 == 0 && Points.Count >= 9; } }
        public List<PointF> Points { get; private set; }
        public List<int> Hull { get; private set; }

        public void Resize(Size newSize)
        {
            for (int i = 0; i < Points.Count; i++)
                Points[i] = ResizePoint(Points[i], newSize);
            currentSize = newSize;
        }

        private PointF ResizePoint(PointF point, Size newSize)
        {
            return new PointF(
                point.X * newSize.Width / currentSize.Width,
                point.Y * newSize.Height / currentSize.Height);
        }

        public void Clear()
        {
            Points.Clear();
            Hull = null;
        }

        public void BuildHull()
        {
            if (!CanBuildHull)
                return;
            foreach (var candidate in FullTraversalHelper.FullTraversal(Points.Count, Points.Count / 3))
            {
                var polygon = new Polygon(candidate.Select(i => Points[i]).ToList());
                if (polygon.IsSimple() && DividePointsEqually(candidate, polygon))
                {
                    Hull = candidate;
                    return;
                }
            }
        }

        private bool DividePointsEqually(List<int> candidate, Polygon polygon)
        {
            var pointsNotOnHull = Enumerable.Range(0, Points.Count)
                .Where(i => !candidate.Contains(i))
                .Select(i => Points[i])
                .ToList();
            var pointsInside = pointsNotOnHull.Count(polygon.IsInside);
            var pointsOuntside = pointsNotOnHull.Count(polygon.IsOutside);
            return pointsInside == pointsOuntside && pointsInside == polygon.Points.Count;
        }

        public void AddPoint(PointF point)
        {
            Hull = null;
            Points.Add(point);
        }
    }
}