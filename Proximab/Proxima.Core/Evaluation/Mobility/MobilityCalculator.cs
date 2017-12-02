using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;

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
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="bitBoard">The bitboard.</param>
        /// <returns>The mobility evaluation result.</returns>
        public int Calculate(BitBoard bitBoard)
        {
            var whiteMobility = GetMobilityValue(Color.White, bitBoard);
            var blackMobility = GetMobilityValue(Color.Black, bitBoard);

            return whiteMobility - blackMobility;
        }

        /// <summary>
        /// Calculates a detailed mobility evaluation result.
        /// </summary>
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="bitBoard">The bitboard.</param>
        /// <returns>The detailed mobility evaluation result.</returns>
        public MobilityData CalculateDetailed(BitBoard bitBoard)
        {
            return new MobilityData()
            {
                WhiteMobility = GetMobilityValue(Color.White, bitBoard),
                BlackMobility = GetMobilityValue(Color.Black, bitBoard)
            };
        }

        /// <summary>
        /// Calculates a mobility evaluation result for the specified player by adding number of possible
        /// moves for every piece with the specified color.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="bitBoard">The bitboard.</param>
        /// <returns>The mobility evaluation result for the specified player.</returns>
        private int GetMobilityValue(Color color, BitBoard bitBoard)
        {
            var mobility = 0;
            var array = MobilityValues.GetRatio(color);

            for (int i = 0; i < 64; i++)
            {
                var field = 1ul << i;
                if ((field & bitBoard.Occupancy[(int)color]) != 0)
                {
                    continue;
                }

                var attacksArray = bitBoard.Attacks[i] & bitBoard.Occupancy[(int)color];
                if (attacksArray != 0)
                {
                    mobility += BitOperations.Count(attacksArray) * array[FastArray.GetEvaluationValueIndex(bitBoard.GamePhase, i)];
                }
            }

            return mobility;
        }
    }
}
