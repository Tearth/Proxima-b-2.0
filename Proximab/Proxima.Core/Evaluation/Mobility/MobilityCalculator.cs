using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Mobility
{
    /// <summary>
    /// Represents a set of methods to evaluate mobility.
    /// </summary>
    /// <remarks>
    /// Mobility defines the number of possible moves of pieces. It's very time-consuming to calculate this one,
    /// but allows to stimulate an AI to developing pieces and capturing more space. More space = more ways to attack
    /// the enemy.
    /// </remarks>
    public class MobilityCalculator
    {
        /// <summary>
        /// Calculates a mobility evaluation result.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The mobility evaluation result.</returns>
        public int Calculate(Bitboard bitboard)
        {
            var whiteMobility = GetMobilityValue(bitboard, Color.White);
            var blackMobility = GetMobilityValue(bitboard, Color.Black);

            return whiteMobility - blackMobility;
        }

        /// <summary>
        /// Calculates a detailed mobility evaluation result.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The detailed mobility evaluation result.</returns>
        public MobilityData CalculateDetailed(Bitboard bitboard)
        {
            return new MobilityData
            {
                WhiteMobility = GetMobilityValue(bitboard, Color.White),
                BlackMobility = GetMobilityValue(bitboard, Color.Black)
            };
        }

        /// <summary>
        /// Calculates a mobility evaluation result for the specified player by adding number of possible
        /// moves for every piece with the specified color.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The player color.</param>
        /// <returns>The mobility evaluation result for the specified player.</returns>
        private int GetMobilityValue(Bitboard bitboard, Color color)
        {
            return BitOperations.Count(bitboard.AttacksSummary[(int)color]);
        }
    }
}
