using Proxima.Core.Commons.Colors;
using Proxima.Core.Evaluation.PawnStructure.Chain;
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
            var pawnChainCalculator = new PawnChainCalculator();

            return new PawnStructureResult()
            {
                WhiteDoubledPawns = doubledPawnsCalculator.GetDoubledPawnsValue(Color.White, parameters),
                BlackDoubledPawns = doubledPawnsCalculator.GetDoubledPawnsValue(Color.Black, parameters),

                WhiteIsolatedPawns = isolatedPawnsCalculator.GetIsolatedPawnsValue(Color.White, parameters),
                BlackIsolatedPawns = isolatedPawnsCalculator.GetIsolatedPawnsValue(Color.Black, parameters),

                WhitePawnChain = pawnChainCalculator.GetChainValue(Color.White, parameters),
                BlackPawnChain = pawnChainCalculator.GetChainValue(Color.Black, parameters),
            };
        }
    }
}
