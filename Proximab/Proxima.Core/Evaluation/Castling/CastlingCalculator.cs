using Proxima.Core.Boards;
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
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The castling evaluation result.</returns>
        public int Calculate(Bitboard bitboard)
        {
            var whiteCastling = GetCastlingValue(bitboard, Color.White);
            var blackCastling = GetCastlingValue(bitboard, Color.Black);

            return whiteCastling - blackCastling;
        }

        /// <summary>
        /// Calculates a detailed castling evaluation result based on done (or not) castling.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The detailed (separately for white and black) castling evaluation result.</returns>
        public CastlingData CalculateDetailed(Bitboard bitboard)
        {
            return new CastlingData()
            {
                WhiteCastling = GetCastlingValue(bitboard, Color.White),
                BlackCastling = GetCastlingValue(bitboard, Color.Black)
            };
        }

        /// <summary>
        /// Calculates a evaluation value of castling flags for the specified player.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The player color.</param>
        /// <returns>The evaluation result of the castling flags.</returns>
        private int GetCastlingValue(Bitboard bitboard, Color color)
        {
            if (bitboard.CastlingDone[(int)color])
            {
                return CastlingValues.Ratio[(int)bitboard.GamePhase];
            }

            return 0;
        }
    }
}
