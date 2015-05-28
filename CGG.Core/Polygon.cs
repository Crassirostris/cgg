using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CGG.Core
{
    public class Polygon
    {
        public List<PointF> Points { get; private set; }
        public List<Segment> Segments { get; private set; }

        public Polygon(List<PointF> points)
        {
            Points = points;
            Segments = GetAllSegments(points).ToList();
        }

        private static IEnumerable<Segment> GetAllSegments(List<PointF> polygon)
        {
            return polygon.Select((point, i) => new Segment(point, polygon[(i + 1) % polygon.Count]));
        }
        public bool IsSimple()
        {
            for (int i = 0; i < Segments.Count; i++)
                for (int j = i + 2; j < Segments.Count - (i == 0 ? 1 : 0); ++j)
                    if (Segments[i].IntersectsWith(Segments[j]))
                        return false;
            return true;
        }

        public bool IsInside(PointF point)
        {
            if (PointOnPolygon(point))
                return false;
            return GetIntersectionsCount(point) % 2 == 1;
        }

        public bool IsOutside(PointF point)
        {
            if (PointOnPolygon(point))
                return false;
            return GetIntersectionsCount(point) % 2 == 0;
        }

        private bool PointOnPolygon(PointF point)
        {
            return Segments.Any(segment => segment.Contains(point));
        }

        private int GetIntersectionsCount(PointF point)
        {
            var pointOutsidePolygon = new PointF(Points.Min(p => p.X) - 1, Points.Min(p => p.Y) - 1);
            var segment = new Segment(pointOutsidePolygon, point);
            return Segments.Count(s => s.IntersectsWith(segment)) - Points.Count(p => segment.Contains(p));
        }
    }
}