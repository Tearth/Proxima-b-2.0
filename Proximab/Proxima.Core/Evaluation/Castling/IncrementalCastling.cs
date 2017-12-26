using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Castling
{
    /// <summary>
    /// Represents a set of methods to operate on incremental castling evaluation results.
    /// </summary>
    public static class IncrementalCastling
    {
        /// <summary>
        /// Calculates a new castling evaluation result based on information that the specified player has done castling.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The player color.</param>
        public static void SetCastlingDone(Bitboard bitboard, Color color)
        {
            switch (color)
            {
                case Color.White:
                {
                    bitboard.IncEvaluation.Castling += CastlingValues.Ratio[(int)bitboard.GamePhase];
                    break;
                }

                case Color.Black:
                {
                    bitboard.IncEvaluation.Castling -= CastlingValues.Ratio[(int)bitboard.GamePhase];
                    break;
                }
            }
        }
    }
}
