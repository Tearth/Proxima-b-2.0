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
    /// Represents en passant 
    /// </summary>
    public class EnPassantMove : Move
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnPassantMove"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="piece">The piece type.</param>
        /// <param name="color">The piece color.</param>
        public EnPassantMove(Position from, Position to, PieceType piece, Color color)
            : base(from, to, piece, color)
        {
        }

        /// <summary>
        /// Calculates en passant move.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        public override void CalculateMove(Bitboard bitboard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);
            var enemyColor = ColorOperations.Invert(Color);

            RemoveEnPassantPiece(bitboard, enemyColor, to);
            CalculatePieceMove(bitboard, from, to);
        }

        /// <summary>
        /// Removes en passant piece from the specified bitboard.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="enemyColor">The enemy color.</param>
        /// <param name="fieldLSB">The bitboard with set field.</param>
        private void RemoveEnPassantPiece(Bitboard bitboard, Color enemyColor, ulong fieldLSB)
        {
            var enPassantPiece = Color == Color.White ? fieldLSB >> 8 : fieldLSB << 8;

            bitboard.Pieces[FastArray.GetPieceIndex(enemyColor, PieceType.Pawn)] &= ~enPassantPiece;
            bitboard.Occupancy[(int)enemyColor] ^= enPassantPiece;

            IncrementalMaterial.RemovePiece(bitboard, enemyColor, PieceType.Pawn);
            IncrementalPosition.RemovePiece(bitboard, enemyColor, PieceType.Pawn, enPassantPiece);
            IncrementalZobrist.AddOrRemovePiece(enemyColor, PieceType.Pawn, enPassantPiece, bitboard);
        }
    }
}
