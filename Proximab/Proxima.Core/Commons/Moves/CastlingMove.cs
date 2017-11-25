using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Commons.Moves
{
    public class CastlingMove : Move
    {
        public CastlingType CastlingType { get; private set; }

        public CastlingMove(Position from, Position to, PieceType piece, Color color, CastlingType castlingType) 
            : base(from, to, piece, color)
        {
            CastlingType = castlingType;
        }
    }
}
