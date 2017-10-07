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
    }
}
