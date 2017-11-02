using Proxima.Core.Evaluation.Material;

namespace Proxima.Core.Evaluation
{
    public class EvaluationCalculator
    {
        MaterialCalculator _materialCalculator;

        public EvaluationCalculator()
        {
            _materialCalculator = new MaterialCalculator();
        }

        public EvaluationResult GetEvaluation(EvaluationParameters parameters)
        {
            return new EvaluationResult()
            {
                Material = _materialCalculator.Calculate(parameters)
            };
        }
    }
}
