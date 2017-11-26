using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Commons.Moves
{
    /// <summary>
    /// Represents a promotion move.
    /// </summary>
    public class PromotionMove : Move
    {
        /// <summary>
        /// Gets the promotion piece type.
        /// </summary>
        public PieceType PromotionPiece { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionMove"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="piece">The piece type.</param>
        /// <param name="color">The piece color.</param>
        /// <param name="promotionPiece">The piece to which the pawn will be promoted.</param>
        public PromotionMove(Position from, Position to, PieceType piece, Color color, PieceType promotionPiece) 
            : base(from, to, piece, color)
        {
            PromotionPiece = promotionPiece;
        }
    }
}
