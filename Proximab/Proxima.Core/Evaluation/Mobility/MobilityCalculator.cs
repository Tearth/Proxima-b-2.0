using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Mobility
{
    public class MobilityCalculator
    {
        public MobilityResult Calculate(EvaluationParameters parameters)
        {
            var mobility = new MobilityResult();

            mobility.WhiteMobility = GetMobility(Color.White, GetAttacks(Color.White, parameters.AttacksSummary, parameters.Occupancy));
            mobility.BlackMobility = GetMobility(Color.Black, GetAttacks(Color.Black, parameters.AttacksSummary, parameters.Occupancy));

            return mobility;
        }

        ulong GetAttacks(Color color, ulong[] attacksSummary, ulong[] occupancy)
        {
            return attacksSummary[(int)color] & ~occupancy[(int)color];
        }

        int GetMobility(Color color, ulong attacksSummary)
        {
            var mobility = 0;

            while(attacksSummary != 0)
            {
                BitOperations.GetLSB(ref attacksSummary);
                mobility++;
            }

            return mobility * EvaluationConstants.MobilityRatio;
        }
    }
}
