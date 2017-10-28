using System.Diagnostics;

namespace Proxima.Core.Commons.Positions
{
    [DebuggerDisplay("[{X} {Y}]")]
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position()
        {

        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsValid()
        {
            return X >= 1 && X <= 8 && Y >= 1 && Y <= 8;
        }

        public static bool operator ==(Position a, Position b)
        {
            if (ReferenceEquals(null, b))
                return ReferenceEquals(null, a);

            return (a.X == b.X) && (a.Y == b.Y);
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }

        public override bool Equals(object obj)
        {
            var position = obj as Position;
            return position != null &&
                   X == position.X &&
                   Y == position.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
