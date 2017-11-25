using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Position.Values
{
    public static class KnightValues
    {
        public static readonly int[] Pattern = new int[2 * 64]
        {
            // Regular
            -50, -40, -30, -30, -30, -30, -40, -50,
            -40, -20,  0,   0,   0,   0,  -20, -40,
            -30,  0,   10,  15,  15,  10,  0,  -30,
            -30,  5,   15,  20,  20,  15,  5,  -30,
            -30,  0,   15,  20,  20,  15,  0,  -30,
            -30,  5,   10,  15,  15,  10,  5,  -30,
            -40, -20,  0,   5,   5,   0,  -20, -40,
            -50, -40, -30, -30, -30, -30, -40, -50,

            // End
            -50, -40, -30, -30, -30, -30, -40, -50,
            -40, -20,  0,   0,   0,   0,  -20, -40,
            -30,  0,   10,  15,  15,  10,  0,  -30,
            -30,  5,   15,  20,  20,  15,  5,  -30,
            -30,  0,   15,  20,  20,  15,  0,  -30,
            -30,  5,   10,  15,  15,  10,  5,  -30,
            -40, -20,  0,   5,   5,   0,  -20, -40,
            -50, -40, -30, -30, -30, -30, -40, -50,
        };

        public static readonly int[] WhiteValues = EvaluationFlipper.CalculateWhiteArray(Pattern);
        public static readonly int[] BlackValues = EvaluationFlipper.CalculateBlackArray(Pattern);

        public static int[] GetValues(Color color)
        {
            return color == Color.White ? WhiteValues : BlackValues;
        }
    }
}
