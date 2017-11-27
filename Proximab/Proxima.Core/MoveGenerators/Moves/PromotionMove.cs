using Proxima.Core.Boards;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Evaluation;
using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Position;

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
            var change = from | to;

            bitBoard.Pieces[FastArray.GetPieceIndex(Color, Piece)] &= ~from;
            bitBoard.Pieces[FastArray.GetPieceIndex(Color, PromotionPiece)] |= to;
            bitBoard.Occupancy[(int)Color] ^= change;

            bitBoard.IncrementalEvaluation.Material = IncrementalMaterial.RemovePiece(bitBoard.IncrementalEvaluation.Material, Piece, Color);
            bitBoard.IncrementalEvaluation.Material = IncrementalMaterial.AddPiece(bitBoard.IncrementalEvaluation.Material, PromotionPiece, Color);

            bitBoard.IncrementalEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncrementalEvaluation.Position, Color, Piece, from, GamePhase.Regular);
            bitBoard.IncrementalEvaluation.Position = IncrementalPosition.AddPiece(bitBoard.IncrementalEvaluation.Position, Color, PromotionPiece, to, GamePhase.Regular);

            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, Piece, from);
            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, PromotionPiece, to);
        }
    }
}
