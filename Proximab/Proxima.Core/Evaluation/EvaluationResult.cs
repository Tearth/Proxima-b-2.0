using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Mobility;

namespace Proxima.Core.Evaluation
{
    public class EvaluationResult
    {
        public MaterialResult Material { get; set; }
        public MobilityResult Mobility { get; set; }

        public int Total
        {
            get
            {
                return Material.Difference +
                       Mobility.Difference;
            }
        }
    }
}
