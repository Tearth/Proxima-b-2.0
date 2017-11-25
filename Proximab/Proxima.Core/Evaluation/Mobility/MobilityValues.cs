using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Mobility
{
    public static class MobilityValues
    {
        public static readonly int[] RatioPattern = new int[2 * 64]
        {
            // Regular
             1,   1,   1,   1,   1,   1,   1,   1,
             1,   1,   1,   1,   1,   1,   1,   1,
             1,   1,   2,   2,   2,   2,   1,   1,
             1,   1,   2,   4,   4,   2,   1,   1,
             1,   1,   2,   4,   4,   2,   1,   1,
             1,   1,   2,   2,   2,   2,   1,   1,
             1,   1,   1,   1,   1,   1,   1,   1,
             1,   1,   1,   1,   1,   1,   1,   1,

             // End
             1,   1,   1,   1,   1,   1,   1,   1,
             1,   1,   1,   1,   1,   1,   1,   1,
             1,   1,   1,   1,   1,   1,   1,   1,
             1,   1,   1,   1,   1,   1,   1,   1,
             1,   1,   1,   1,   1,   1,   1,   1,
             1,   1,   1,   1,   1,   1,   1,   1,
             1,   1,   1,   1,   1,   1,   1,   1,
             1,   1,   1,   1,   1,   1,   1,   1,
        };

        public static readonly int[] WhiteRatio = EvaluationFlipper.CalculateWhiteArray(RatioPattern);
        public static readonly int[] BlackRatio = EvaluationFlipper.CalculateBlackArray(RatioPattern);

        public static int[] GetRatio(Color color)
        {
            return color == Color.White ? WhiteRatio : BlackRatio;
        }
    }
}
