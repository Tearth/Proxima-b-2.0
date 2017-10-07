using Core.Commons.Positions;

namespace Core.Commons.Moves
{
    public class Move
    {
        public Position From { get; set; }
        public Position To { get; set; }

        public MoveType Type { get; set; }

        public Move() : this(new Position(1, 1), new Position(1, 1), MoveType.None)
        {

        }

        public Move(Position from, Position to, MoveType type)
        {
            From = from;
            To = to;
            Type = type;
        }
    }
}
