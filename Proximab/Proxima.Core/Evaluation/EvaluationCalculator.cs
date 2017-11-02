using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Mobility;

namespace Proxima.Core.Evaluation
{
    public class EvaluationCalculator
    {
        MaterialCalculator _materialCalculator;
        MobilityCalculator _mobilityCalculator;

        public EvaluationCalculator()
        {
            _materialCalculator = new MaterialCalculator();
            _mobilityCalculator = new MobilityCalculator();
        }

        public EvaluationResult GetEvaluation(EvaluationParameters parameters)
        {
            return new EvaluationResult()
            {
                Material = _materialCalculator.Calculate(parameters),
                Mobility = _mobilityCalculator.Calculate(parameters)
            };
        }
    }
}
