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
    /// Represents a kill move (on destination position is another enemy piece).
    /// </summary>
    public class KillMove : Move
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KillMove"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="piece">The piece type.</param>
        /// <param name="color">The piece color.</param>
        public KillMove(Position from, Position to, PieceType piece, Color color) 
            : base(from, to, piece, color)
        {
        }

        public override void CalculateMove(BitBoard bitBoard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);
            var enemyColor = ColorOperations.Invert(Color);

            RemovePiece(bitBoard, enemyColor, to);
            CalculatePieceMove(bitBoard, from, to);
        }

        private static void RemovePiece(BitBoard bitBoard, Color enemyColor, ulong fieldLSB)
        {
            for (int piece = 0; piece < 6; piece++)
            {
                var index = FastArray.GetPieceIndex(enemyColor, (PieceType)piece);
                if ((bitBoard.Pieces[index] & fieldLSB) != 0)
                {
                    bitBoard.Pieces[index] &= ~fieldLSB;
                    bitBoard.Occupancy[(int)enemyColor] &= ~fieldLSB;

                    bitBoard.IncEvaluation.Material = IncrementalMaterial.RemovePiece(bitBoard.IncEvaluation.Material, (PieceType)piece, enemyColor);
                    bitBoard.IncEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncEvaluation.Position, enemyColor, (PieceType)piece, fieldLSB, GamePhase.Regular);
                    bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, enemyColor, (PieceType)piece, fieldLSB);

                    break;
                }
            }
        }
    }
}
