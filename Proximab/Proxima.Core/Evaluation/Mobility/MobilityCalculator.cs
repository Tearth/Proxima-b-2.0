using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Evaluation.Mobility
{
    public class MobilityCalculator
    {
        public int Calculate(GamePhase gamePhase, BitBoard bitBoard)
        {
            var whiteMobility = GetMobilityValue(Color.White, gamePhase, bitBoard);
            var blackMobility = GetMobilityValue(Color.Black, gamePhase, bitBoard);

            return whiteMobility - blackMobility;
        }

        public MobilityData CalculateDetailed(GamePhase gamePhase, BitBoard bitBoard)
        {
            return new MobilityData()
            {
                WhiteMobility = GetMobilityValue(Color.White, gamePhase, bitBoard),
                BlackMobility = GetMobilityValue(Color.Black, gamePhase, bitBoard)
            };
        }

        private int GetMobilityValue(Color color, GamePhase gamePhase, BitBoard bitBoard)
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
                    mobility += BitOperations.Count(attacksArray) * array[FastArray.GetEvaluationValueIndex(gamePhase, i)];
                }
            }

            return mobility;
        }
    }
}
