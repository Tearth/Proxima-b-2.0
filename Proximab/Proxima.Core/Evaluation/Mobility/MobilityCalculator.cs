using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Evaluation.Mobility
{
    public class MobilityCalculator
    {
        public MobilityData Calculate(EvaluationParameters parameters)
        {
            return new MobilityData()
            {
                WhiteMobility = GetMobilityValue(Color.White, parameters),
                BlackMobility = GetMobilityValue(Color.Black, parameters)
            };
        }

        int GetMobilityValue(Color color, EvaluationParameters parameters)
        {
            var mobility = 0;
            var array = MobilityValues.GetRatio(color);

            for (int i = 0; i < 64; i++)
            {
                var field = 1ul << i;
                if ((field & parameters.Occupancy[(int)color]) != 0)
                    continue;

                var attacksArray = parameters.Attacks[i] & parameters.Occupancy[(int)color];
                if(attacksArray != 0)
                {
                    mobility += BitOperations.Count(attacksArray) * array[FastArray.GetEvaluationValueIndex(parameters.GamePhase, i)];
                }
            }
       
            return mobility;
        }
    }
}
