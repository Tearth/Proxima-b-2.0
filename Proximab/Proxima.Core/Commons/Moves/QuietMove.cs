using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Commons.Moves
{
    public class QuietMove : Move
    {
        public QuietMove(Position from, Position to, PieceType piece, Color color) 
            : base(from, to, piece, color)
        {
        }
    }
}
