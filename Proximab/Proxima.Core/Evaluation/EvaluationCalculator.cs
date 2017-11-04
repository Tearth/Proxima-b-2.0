using Proxima.Core.Evaluation.Castling;
using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Mobility;

namespace Proxima.Core.Evaluation
{
    public class EvaluationCalculator
    {
        MaterialCalculator _materialCalculator;
        MobilityCalculator _mobilityCalculator;
        CastlingCalculator _castlingCalculator;

        public EvaluationCalculator()
        {
            _materialCalculator = new MaterialCalculator();
            _mobilityCalculator = new MobilityCalculator();
            _castlingCalculator = new CastlingCalculator();
        }

        public EvaluationResult GetEvaluation(EvaluationParameters parameters)
        {
            return new EvaluationResult()
            {
                Material = new MaterialResult(),
                Mobility = new MobilityResult(),
                Castling = _castlingCalculator.Calculate(parameters)
            };
        }
    }
}
