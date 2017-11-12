using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Mobility
{
    public class MobilityCalculator
    {
        public MobilityResult Calculate(EvaluationParameters parameters)
        {
            return new MobilityResult()
            {
                WhiteMobility = GetMobilityValue(Color.White, parameters),
                BlackMobility = GetMobilityValue(Color.Black, parameters)
            };
        }

        int GetMobilityValue(Color color, EvaluationParameters parameters)
        {
            var mobility = 0;
            var fieldsToParse = ~parameters.Occupancy[(int)color];

            for (int i = 0; i < 64; i++)
            {
                var field = 1ul << i;
                if ((field & parameters.Occupancy[(int)color]) != 0)
                    continue;

                var attacksArray = parameters.Attacks[i] & parameters.Occupancy[(int)color];
                if(attacksArray != 0)
                {
                    mobility += BitOperations.Count(attacksArray) * GetMobilityRatio(field, parameters.GamePhase);
                }
            }
       
            return mobility;
        }
        
        int GetMobilityRatio(ulong field, GamePhase gamePhase)
        {
            if      ((field & BitConstants.SmallCenter) != 0) return MobilityValues.SmallCenterRatio[(int)gamePhase];
            else if ((field & BitConstants.BigCenter)   != 0) return MobilityValues.BigCenterRatio[(int)gamePhase];

            return MobilityValues.Ratio[(int)gamePhase];
        }
    }
}
