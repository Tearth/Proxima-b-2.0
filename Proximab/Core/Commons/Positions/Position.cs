using Core.Commons.Exceptions;

namespace Core.Commons.Positions
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position() : this(1, 1)
        {

        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;

            if (!IsValid())
                throw new PositionOutOfRangeException();
        }

        bool IsValid()
        {
            return (X >= 1 && X <= 8) && (Y >= 1 && Y <= 8);
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
