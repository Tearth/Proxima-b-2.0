using Proxima.Core.Evaluation.Calculators;

namespace Proxima.Core.Evaluation
{
    public class EvaluationResult
    {
        public MaterialResult Material { get; set; }

        public int Total
        {
            get
            {
                return Material.Difference;
            }
        }
    }
}
