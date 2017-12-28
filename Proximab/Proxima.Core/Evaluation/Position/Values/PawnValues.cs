using System.Diagnostics.CodeAnalysis;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Position.Values
{
    /// <summary>
    /// Represents a set of evaluation parameters for pawn position.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class PawnValues
    {
        public static readonly int[] Pattern =
        {
            // Regular
            0,   0,   0,   0,   0,   0,   0,   0,
            50,  50,  50,  50,  50,  50,  50,  50,
            10,  10,  20,  30,  30,  20,  10,  10,
            5,   5,   10,  25,  25,  10, -10, -20,
            0,   0,   0,   20,  20, -30, -30, -30,
            5,  -5,  -10,  5,   5,  -30,  20, -30,
            10,  10,  10, -20, -20,  10,  10,  30,
            0,   0,   0,   0,   0,   0,   0,   0,

            // End
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
