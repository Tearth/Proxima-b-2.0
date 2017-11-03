using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Mobility
{
    public class MobilityCalculator
    {
        public MobilityResult Calculate(EvaluationParameters parameters)
        {
            var mobility = new MobilityResult();

            mobility.WhiteMobility = GetMobility(Color.White, parameters.Attacks, parameters.Occupancy);
            mobility.BlackMobility = GetMobility(Color.Black, parameters.Attacks, parameters.Occupancy);

            return mobility;
        }

        int GetMobility(Color color, ulong[] attacks, ulong[] occupancy)
        {
            var mobility = 0;

            for (int i = 0; i < 64; i++)
            {
                var field = 1ul << i;
                if ((field & occupancy[(int)color]) != 0)
                    continue;

                var attacksArray = attacks[i] & occupancy[(int)color];
                var mobilityRatio = GetMobilityRatio(field);

                mobility += BitOperations.Count(attacksArray) * mobilityRatio;
            }
       
            return mobility;
        }

        int GetMobilityRatio(ulong field)
        {
            if      ((field & BitConstants.SmallCenter) != 0) return EvaluationConstants.MobilitySmallCenterRatio;
            else if ((field & BitConstants.BigCenter)   != 0) return EvaluationConstants.MobilityBigCenterRatio;

            return EvaluationConstants.MobilityRatio;
        }
    }
}
