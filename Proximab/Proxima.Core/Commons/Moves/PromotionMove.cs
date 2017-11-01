using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Commons.Moves
{
    public class PromotionMove : Move
    {
        public PieceType PromotionPiece { get; private set; }

        public PromotionMove(Position from, Position to, PieceType piece, Color color, PieceType promotionPiece) 
            : base(from, to, piece, color)
        {
            PromotionPiece = promotionPiece;
        }
    }
}
