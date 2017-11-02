using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation
{
    public class EvaluationResult
    {
        public int[] Material { get; set; }

        public int Total
        {
            get
            {
                return (Material[(int)Color.White] - Material[(int)Color.Black]);
            }
        }
    }
}
