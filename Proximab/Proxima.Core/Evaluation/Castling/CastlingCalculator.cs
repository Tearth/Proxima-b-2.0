using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Castling
{
    /// <summary>
    /// Represents a set of methods to evaluate castling flags.
    /// </summary>
    /// <remarks>
    /// Castling is one of the most important parts of the game if we think about king security.
    /// Done castling is rewarded by positive points (same for short and long).
    /// </remarks>
    public class CastlingCalculator
    {
        /// <summary>
        /// Calculates a castling evaluation result based on done (or not) castling.
        /// </summary>
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The castling evaluation result.</returns>
        public int Calculate(Bitboard bitboard)
        {
            var whiteCastling = GetCastlingValue(Color.White, bitboard);
            var blackCastling = GetCastlingValue(Color.Black, bitboard);

            return whiteCastling - blackCastling;
        }

        /// <summary>
        /// Calculates a detailed castling evaluation result based on done (or not) castling.
        /// </summary>
        /// <param name="gamePhase">THe game phase.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The detailed (separately for white and black player) castling evaluation result.</returns>
        public CastlingData CalculateDetailed(Bitboard bitboard)
        {
            return new CastlingData()
            {
                WhiteCastling = GetCastlingValue(Color.White, bitboard),
                BlackCastling = GetCastlingValue(Color.Black, bitboard)
            };
        }

        private int GetCastlingValue(Color color, Bitboard bitboard)
        {
            if (bitboard.CastlingDone[(int)color])
            {
                return CastlingValues.Ratio[(int)bitboard.GamePhase];
            }

            return 0;
        }
    }
}
