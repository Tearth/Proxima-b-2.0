using Proxima.Core.Commons.Colors;
using Proxima.Core.Evaluation.PawnStructure.Doubled;
using Proxima.Core.Evaluation.PawnStructure.Isolated;

namespace Proxima.Core.Evaluation.PawnStructure
{
    public class PawnStructureCalculator
    {
        public PawnStructureResult Calculate(EvaluationParameters parameters)
        {
            var doubledPawnsCalculator = new DoubledPawnsCalculator();
            var isolatedPawnsCalculator = new IsolatedPawnsCalculator();

            return new PawnStructureResult()
            {
                WhiteDoubledPawns = doubledPawnsCalculator.GetDoubledPawns(Color.White, parameters),
                BlackDoubledPawns = doubledPawnsCalculator.GetDoubledPawns(Color.Black, parameters),

                WhiteIsolatedPawns = isolatedPawnsCalculator.GetIsolatedPawns(Color.White, parameters),
                BlackIsolatedPawns = isolatedPawnsCalculator.GetIsolatedPawns(Color.Black, parameters)
            };
        }
    }
}
