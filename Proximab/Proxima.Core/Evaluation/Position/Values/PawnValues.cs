using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Position.Values
{
    public static class PawnValues
    {
        public static readonly int[] Pattern = new int[2 * 64]
        {
            //Regular
            0,   0,   0,   0,   0,   0,   0,   0,
            50,  50,  50,  50,  50,  50,  50,  50,
            10,  10,  20,  30,  30,  20,  10,  10,
            5,   5,   10,  25,  25,  10,  5,   5,
            0,   0,   0,   20,  20,  0,   0,   0,
            5,  -5,  -10,  0,   0,  -10, -5,   5,
            5,   10,  10, -20, -20,  10,  10,  5,
            0,   0,   0,   0,   0,   0,   0,   0,

            //End
            0,   0,   0,   0,   0,   0,   0,   0,
            50,  50,  50,  50,  50,  50,  50,  50,
            10,  10,  20,  30,  30,  20,  10,  10,
            5,   5,   10,  25,  25,  10,  5,   5,
            0,   0,   0,   20,  20,  0,   0,   0,
            5,  -5,  -10,  0,   0,  -10, -5,   5,
            5,   10,  10, -20, -20,  10,  10,  5,
            0,   0,   0,   0,   0,   0,   0,   0
        };

        public static readonly int[] WhiteValues = EvaluationFlipper.CalculateWhiteArray(Pattern);
        public static readonly int[] BlackValues = EvaluationFlipper.CalculateBlackArray(Pattern);

        public static int[] GetValues(Color color)
        {
            return color == Color.White ? WhiteValues : BlackValues;
        }
    }
}
