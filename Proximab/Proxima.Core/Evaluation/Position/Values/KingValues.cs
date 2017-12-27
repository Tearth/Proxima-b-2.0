using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Position.Values
{
    /// <summary>
    /// Represents a set of evaluation parameters for king position.
    /// </summary>
    public static class KingValues
    {
        public static readonly int[] Pattern = new int[]
        {
            // Regular
            -30, -40, -40, -50, -50, -40, -40, -30,
            -30, -40, -40, -50, -50, -40, -40, -30,
            -30, -40, -40, -50, -50, -40, -40, -30,
            -30, -40, -40, -50, -50, -40, -40, -30,
            -20, -30, -30, -40, -40, -30, -30, -20,
            -10, -20, -20, -20, -20, -20, -20, -10,
             20,  20,   0,   0,   0,   0,  20,  20,
             0,   10,  30, -10,   0, -10,  30,   0,

            // End
            -30, -20, -20, -20, -20, -20, -20, -30,
            -30,   0,   0,   0,   0,   0,   0, -30,
            -30,   0,  10,  10,  10,  10,   0, -30,
            -30,   0,  10,   0,   0,  10,   0, -30,
            -30,   0,  10,   0,   0,  10,   0, -30,
            -30,   0,  10,  10,  10,  10,   0, -30,
            -30,   0,   0,   0,   0,   0,   0, -30,
            -30, -20, -20, -20, -20, -20, -20, -30,
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
