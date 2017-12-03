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

        public override void CalculateMove(Bitboard bitboard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);
            var enemyColor = ColorOperations.Invert(Color);

            RemovePiece(bitboard, enemyColor, to);
            CalculatePieceMove(bitboard, from, to);
        }

        private static void RemovePiece(Bitboard bitboard, Color enemyColor, ulong fieldLSB)
        {
            for (int piece = 0; piece < 6; piece++)
            {
                var index = FastArray.GetPieceIndex(enemyColor, (PieceType)piece);
                if ((bitboard.Pieces[index] & fieldLSB) != 0)
                {
                    bitboard.Pieces[index] &= ~fieldLSB;
                    bitboard.Occupancy[(int)enemyColor] &= ~fieldLSB;

                    IncrementalMaterial.RemovePiece(bitboard, enemyColor, (PieceType)piece);
                    IncrementalPosition.RemovePiece(bitboard, enemyColor, (PieceType)piece, fieldLSB);
                    IncrementalZobrist.AddOrRemovePiece(enemyColor, (PieceType)piece, fieldLSB, bitboard);

                    break;
                }
            }
        }
    }
}
