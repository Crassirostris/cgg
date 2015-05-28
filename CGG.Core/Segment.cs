using System;
using System.Drawing;

namespace CGG.Core
{
    public struct Segment : IEquatable<Segment>
    {
        public PointF From { get; private set; }
        public PointF To { get; private set; }
        public PointF Direction { get; private set; }

        public Segment(PointF @from, PointF to)
            : this()
        {
            From = @from;
            To = to;
            Direction = From.Substract(To);
        }

        public bool IntersectsWith(Segment otherSegment)
        {
            if (!BoundingBoxIntersects(otherSegment))
                return false;
            if (CollinearWith(otherSegment))
                return true;
            return Divides(otherSegment) && otherSegment.Divides(this);
        }

        public bool Contains(PointF point)
        {
            return BoundingBoxContains(point) && CollinearWith(new Segment(From, point));
        }

        private bool Divides(Segment other)
        {
            var a = other.From.Substract(From);
            var b = other.To.Substract(From);
            return Direction.CrossProduct(a) * Direction.CrossProduct(b) < float.Epsilon;
        }

        private bool CollinearWith(Segment other)
        {
            return Math.Abs(Direction.X * other.Direction.Y - Direction.Y * other.Direction.X) < float.Epsilon;
        }

        private bool BoundingBoxIntersects(Segment other)
        {
            return Intersects(
                    Math.Min(From.X, To.X), Math.Max(From.X, To.X),
                    Math.Min(other.From.X, other.To.X), Math.Max(other.From.X, other.To.X))
                && Intersects(
                    Math.Min(From.Y, To.Y), Math.Max(From.Y, To.Y),
                    Math.Min(other.From.Y, other.To.Y), Math.Max(other.From.Y, other.To.Y));
        }

        private bool Intersects(float left1, float right1, float left2, float right2)
        {
            return Between(left1, left2, right2)
                   || Between(right1, left2, right2)
                   || Between(left2, left1, right1)
                   || Between(right2, left1, right1);
        }

        private bool Between(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        private bool BoundingBoxContains(PointF point)
        {
            return point.X >= Math.Min(From.X, To.X) && point.X <= Math.Max(From.X, To.X)
                   && point.Y >= Math.Min(From.Y, To.Y) && point.Y <= Math.Max(From.Y, To.Y);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}}}", From, To);
        }

        #region Equality Members

        public bool Equals(Segment other)
        {
            return From.Equals(other.From) && To.Equals(other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Segment && Equals((Segment)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (From.GetHashCode() * 397) ^ To.GetHashCode();
            }
        }

        public static bool operator ==(Segment left, Segment right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Segment left, Segment right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}