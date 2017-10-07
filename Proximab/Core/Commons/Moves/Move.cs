using Core.Commons.Colors;
using Core.Commons.Positions;

namespace Core.Commons.Moves
{
    public class Move
    {
        public Position From { get; set; }
        public Position To { get; set; }

        public Color Color { get; set; }
        public MoveType Type { get; set; }

        public Move() : this(new Position(1, 1), new Position(1, 1), Color.None, MoveType.None)
        {

        }

        public Move(Position from, Position to, Color color, MoveType type)
        {
            From = from;
            To = to;
            Color = color;
            Type = type;
        }
    }
}
