using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Evaluation.Material;

namespace Proxima.Core.MoveGenerators.Moves
{
    /// <summary>
    /// Represents a promotion 
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

        public override void CalculateMove(BitBoard bitBoard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);

            CalculatePieceMove(bitBoard, Piece, from, PromotionPiece, to);

            bitBoard.IncrementalEvaluation.Material = IncrementalMaterial.RemovePiece(bitBoard.IncrementalEvaluation.Material, Piece, Color);
            bitBoard.IncrementalEvaluation.Material = IncrementalMaterial.AddPiece(bitBoard.IncrementalEvaluation.Material, PromotionPiece, Color);
        }
    }
}