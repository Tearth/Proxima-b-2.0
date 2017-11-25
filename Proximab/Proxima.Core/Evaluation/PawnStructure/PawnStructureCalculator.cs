using Proxima.Core.Commons.Colors;
using Proxima.Core.Evaluation.PawnStructure.Chain;
using Proxima.Core.Evaluation.PawnStructure.Doubled;
using Proxima.Core.Evaluation.PawnStructure.Isolated;

namespace Proxima.Core.Evaluation.PawnStructure
{
    public class PawnStructureCalculator
    {
        private DoubledPawnsCalculator _doubledPawnsCalculator;
        private IsolatedPawnsCalculator _isolatedPawnsCalculator;
        private PawnChainCalculator _pawnChainCalculator;

        public PawnStructureCalculator()
        {
            _doubledPawnsCalculator = new DoubledPawnsCalculator();
            _isolatedPawnsCalculator = new IsolatedPawnsCalculator();
            _pawnChainCalculator = new PawnChainCalculator();
        }

        public int Calculate(EvaluationParameters parameters)
        {
            var whiteDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(Color.White, parameters);
            var blackDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(Color.Black, parameters);

            var whiteIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(Color.White, parameters);
            var blackIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(Color.Black, parameters);

            var whitePawnChains = _pawnChainCalculator.GetChainValue(Color.White, parameters);
            var blackPawnChains = _pawnChainCalculator.GetChainValue(Color.Black, parameters);

            return (whiteDoubledPawns - blackDoubledPawns) + 
                   (whiteIsolatedPawns - blackIsolatedPawns) + 
                   (whitePawnChains - blackPawnChains);
        }

        public PawnStructureData CalculateDetailed(EvaluationParameters parameters)
        {
            return new PawnStructureData()
            {
                WhiteDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(Color.White, parameters),
                BlackDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(Color.Black, parameters),

                WhiteIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(Color.White, parameters),
                BlackIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(Color.Black, parameters),

                WhitePawnChain = _pawnChainCalculator.GetChainValue(Color.White, parameters),
                BlackPawnChain = _pawnChainCalculator.GetChainValue(Color.Black, parameters),
            };
        }
    }
}
