using System.Diagnostics;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Commons.Moves
{
    [DebuggerDisplay("{Color} {Piece} from [{From.X} {From.Y}] to [{To.X} {To.Y}]")]
    public class Move
    {
        public Position From { get; private set; }
        public Position To { get; private set; }

        public PieceType Piece { get; private set; }
        public Color Color { get; private set; }

        public Move()
        {
        }

        public Move(Position from, Position to, PieceType piece, Color color)
        {
            From = from;
            To = to;
            Piece = piece;
            Color = color;
        }

        public bool IsValid()
        {
            return From.IsValid() && To.IsValid();
        }
    }
}
