using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Commons.Moves
{
    public class EnPassantMove : Move
    {
        public EnPassantMove(Position from, Position to, PieceType piece, Color color) 
            : base(from, to, piece, color)
        {
        }
    }
}
