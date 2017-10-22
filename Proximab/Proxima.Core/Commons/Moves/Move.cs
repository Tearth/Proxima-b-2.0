using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using System.Diagnostics;

namespace Proxima.Core.Commons.Moves
{
    [DebuggerDisplay("{Color} {Piece} from [{From.X} {From.Y}] to [{To.X} {To.Y}]")]
    public class Move
    {
        public Position From { get; set; }
        public Position To { get; set; }

        public PieceType Piece { get; set; }
        public Color Color { get; set; }
        public MoveType Type { get; set; }

        public Move()
        {

        }

        public Move(Position from, Position to, PieceType piece, Color color, MoveType type)
        {
            From = from;
            To = to;
            Piece = piece;
            Color = color;
            Type = type;
        }
    }
}
