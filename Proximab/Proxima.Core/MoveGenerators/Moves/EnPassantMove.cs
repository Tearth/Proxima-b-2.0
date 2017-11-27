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
            var change = from | to;

            var enemyColor = ColorOperations.Invert(Color);

            if (Color == Color.White)
            {
                bitBoard.Pieces[FastArray.GetPieceIndex(enemyColor, PieceType.Pawn)] &= ~(to >> 8);
                bitBoard.Occupancy[(int)enemyColor] ^= to >> 8;

                bitBoard.IncrementalEvaluation.Material = IncrementalMaterial.RemovePiece(bitBoard.IncrementalEvaluation.Material, PieceType.Pawn, enemyColor);
                bitBoard.IncrementalEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncrementalEvaluation.Position, enemyColor, PieceType.Pawn, to >> 8, GamePhase.Regular);
                bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, enemyColor, PieceType.Pawn, to >> 8);
            }
            else
            {
                bitBoard.Pieces[FastArray.GetPieceIndex(enemyColor, PieceType.Pawn)] &= ~(to << 8);
                bitBoard.Occupancy[(int)enemyColor] ^= to << 8;

                bitBoard.IncrementalEvaluation.Material = IncrementalMaterial.RemovePiece(bitBoard.IncrementalEvaluation.Material, PieceType.Pawn, enemyColor);
                bitBoard.IncrementalEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncrementalEvaluation.Position, enemyColor, PieceType.Pawn, to << 8, GamePhase.Regular);
                bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, enemyColor, PieceType.Pawn, to << 8);
            }

            bitBoard.Pieces[FastArray.GetPieceIndex(Color, Piece)] ^= change;
            bitBoard.Occupancy[(int)Color] ^= change;

            bitBoard.IncrementalEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncrementalEvaluation.Position, Color, Piece, from, GamePhase.Regular);
            bitBoard.IncrementalEvaluation.Position = IncrementalPosition.AddPiece(bitBoard.IncrementalEvaluation.Position, Color, Piece, to, GamePhase.Regular);

            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, Piece, from);
            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, Piece, to);
        }
    }
}
