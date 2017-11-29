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

        public override void CalculateMove(BitBoard bitBoard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);
            var enemyColor = ColorOperations.Invert(Color);

            RemoveEnPassantPiece(bitBoard, enemyColor, to);
            CalculatePieceMove(bitBoard, from, to);
        }

        private void RemoveEnPassantPiece(BitBoard bitBoard, Color enemyColor, ulong fieldLSB)
        {
            var enPassantPiece = Color == Color.White ? fieldLSB >> 8 : fieldLSB << 8;

            bitBoard.Pieces[FastArray.GetPieceIndex(enemyColor, PieceType.Pawn)] &= ~enPassantPiece;
            bitBoard.Occupancy[(int)enemyColor] ^= enPassantPiece;

            bitBoard.IncEvaluation.Material = IncrementalMaterial.RemovePiece(bitBoard.IncEvaluation.Material, PieceType.Pawn, enemyColor);
            bitBoard.IncEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncEvaluation.Position, enemyColor, PieceType.Pawn, enPassantPiece, GamePhase.Regular);
            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, enemyColor, PieceType.Pawn, enPassantPiece);
        }
    }
}
