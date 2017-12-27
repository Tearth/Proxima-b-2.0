using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;
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
        /// Gets a value indicating whether the promotion move is killing enemy piece.
        /// </summary>
        public bool KillMove { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionMove"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="piece">The piece type.</param>
        /// <param name="color">The piece color.</param>
        /// <param name="promotionPiece">The piece to which the pawn will be promoted.</param>
        /// <param name="killMove">The flag indicating whether the promotion move is killing enemy piece.</param>
        public PromotionMove(Position from, Position to, PieceType piece, Color color, PieceType promotionPiece, bool killMove)
            : base(from, to, piece, color)
        {
            PromotionPiece = promotionPiece;
            KillMove = killMove;
        }

        /// <summary>
        /// Calculates a promotion move.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        public override void CalculateMove(Bitboard bitboard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);

            if (KillMove)
            {
                CalculateKill(bitboard, ColorOperations.Invert(Color), to);
            }

            CalculatePieceMove(bitboard, Piece, from, PromotionPiece, to);

            IncrementalMaterial.RemovePiece(bitboard, Color, Piece);
            IncrementalMaterial.AddPiece(bitboard, Color, PromotionPiece);
        }
    }
}