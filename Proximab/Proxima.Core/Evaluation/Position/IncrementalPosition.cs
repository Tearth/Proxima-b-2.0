using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Evaluation.Position.Values;

namespace Proxima.Core.Evaluation.Position
{
    /// <summary>
    /// Represents a set of methods to operate on incremental position evaluation results.
    /// </summary>
    public static class IncrementalPosition
    {
        /// <summary>
        /// Calculates a new position evaluation result based on the value of the new piece and its position.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The piece color.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="piece">The piece bit.</param>
        public static void AddPiece(Bitboard bitboard, Color color, PieceType pieceType, ulong piece)
        {
            var pieceIndex = BitOperations.GetBitIndex(piece);
            var array = PositionValues.GetValues(color, bitboard.GamePhase, pieceType);
            var delta = array[pieceIndex];

            switch (color)
            {
                case Color.White:
                {
                    bitboard.IncEvaluation.Position += delta;
                    break;
                }

                case Color.Black:
                {
                    bitboard.IncEvaluation.Position -= delta;
                    break;
                }
            }
        }

        /// <summary>
        /// Calculates a new position evaluation result based on the value of the removed piece and its position.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The piece color.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="piece">The piece bit.</param>
        public static void RemovePiece(Bitboard bitboard, Color color, PieceType pieceType, ulong piece)
        {
            var pieceIndex = BitOperations.GetBitIndex(piece);
            var array = PositionValues.GetValues(color, bitboard.GamePhase, pieceType);
            var delta = array[pieceIndex];

            switch (color)
            {
                case Color.White:
                {
                    bitboard.IncEvaluation.Position -= delta;
                    break;
                }

                case Color.Black:
                {
                    bitboard.IncEvaluation.Position += delta;
                    break;
                }
            }
        }
    }
}
