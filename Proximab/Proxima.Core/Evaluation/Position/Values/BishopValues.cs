using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Position.Values
{
    public static class BishopValues
    {
        public static readonly int[] Pattern = new int[2 * 64]
        {
            // Regular
            -20, -10, -10, -10, -10, -10, -10, -20,
            -10,  0,   0,   0,   0,   0,   0,  -10,
            -10,  0,   5,   10,  10,  5,   0,  -10,
            -10,  5,   5,   10,  10,  5,   5,  -10,
            -10,  0,   10,  10,  10,  10,  0,  -10,
            -10,  10,  10,  10,  10,  10,  10, -10,
            -10,  5,   0,   0,   0,   0,   5,  -10,
            -20, -10, -10, -10, -10, -10, -10, -20,

            // End
            -20, -10, -10, -10, -10, -10, -10, -20,
            -10,  0,   0,   0,   0,   0,   0,  -10,
            -10,  0,   5,   10,  10,  5,   0,  -10,
            -10,  5,   5,   10,  10,  5,   5,  -10,
            -10,  0,   10,  10,  10,  10,  0,  -10,
            -10,  10,  10,  10,  10,  10,  10, -10,
            -10,  5,   0,   0,   0,   0,   5,  -10,
            -20, -10, -10, -10, -10, -10, -10, -20,
        };

        public static readonly int[] WhiteValues = EvaluationFlipper.CalculateWhiteArray(Pattern);
        public static readonly int[] BlackValues = EvaluationFlipper.CalculateBlackArray(Pattern);

        public static int[] GetValues(Color color)
        {
            return color == Color.White ? WhiteValues : BlackValues;
        }
    }
}
