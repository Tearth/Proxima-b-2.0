using Proxima.Core.Commons;
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
        /// <param name="castling">The current castling evaluation result.</param>
        /// <param name="color">The player color.</param>
        /// <param name="gamePhase">The game phase.</param>
        /// <returns>The updated castling evaluation result.</returns>
        public static int SetCastlingDone(int castling, Color color, GamePhase gamePhase)
        {
            switch (color)
            {
                case Color.White: return castling + CastlingValues.Ratio[(int)gamePhase];
                case Color.Black: return castling - CastlingValues.Ratio[(int)gamePhase];
            }

            return 0;
        }
    }
}
