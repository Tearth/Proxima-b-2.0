using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Position.Values
{
    /// <summary>
    /// Represents a set of evaluation parameters for bishop position.
    /// </summary>
    public static class BishopValues
    {
        public static readonly int[] Pattern =
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
            -20, -10, -10, -10, -10, -10, -10, -20
        };

        public static readonly int[] WhiteValues = EvaluationFlipper.CalculateWhiteArray(Pattern);
        public static readonly int[] BlackValues = EvaluationFlipper.CalculateBlackArray(Pattern);

        /// <summary>
        /// Calculates a position values array for the specified player color.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <returns>The position values array for the specified color.</returns>
        public static int[] GetValues(Color color)
        {
            return color == Color.White ? WhiteValues : BlackValues;
        }
    }
}
