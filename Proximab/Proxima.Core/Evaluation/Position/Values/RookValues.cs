using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Position.Values
{
    /// <summary>
    /// Represents a set of evaluation parameters for rook position.
    /// </summary>
    public static class RookValues
    {
        public static readonly int[] Pattern = new int[]
        {
            // Regular
            0,   0,   0,   0,   0,   0,   0,   0,
            5,   10,  10,  10,  10,  10,  10,  5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
            0,   0,   0,   10,  5,  10,   0,   0,

            // End
            0,   0,   0,   0,   0,   0,   0,   0,
            5,   10,  10,  10,  10,  10,  10,  5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
            0,   0,   0,   10,  5,  10,   0,   0,
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
