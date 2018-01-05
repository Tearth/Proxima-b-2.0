using System.Diagnostics.CodeAnalysis;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Position.Values
{
    /// <summary>
    /// Represents a set of evaluation parameters for king position.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class KingValues
    {
        public static readonly int[][] Pattern =
        {
            // Regular
            new[] { -30, -40, -40, -50, -50, -40, -40, -30,
                    -30, -40, -40, -50, -50, -40, -40, -30,
                    -30, -40, -40, -50, -50, -40, -40, -30,
                    -30, -40, -40, -50, -50, -40, -40, -30,
                    -20, -30, -30, -40, -40, -30, -30, -20,
                    -10, -20, -20, -20, -20, -20, -20, -10,
                    -5,   -5,   0,   0,   0,   0,  -5,  -5,
                     0,   10,  15, -10,   0, -10,  15,   0 },

            // End
            new[] { -10, -10, -10, -10, -10, -10, -10, -10,
                    -10,   0,   0,   0,   0,   0,   0, -10,
                    -10,   0,  10,  10,  10,  10,   0, -10,
                    -10,   0,  10,  15,  15,  10,   0, -10,
                    -10,   0,  10,  15,  15,  10,   0, -10,
                    -10,   0,  10,  10,  10,  10,   0, -10,
                    -10,   0,   0,   0,   0,   0,   0, -10,
                    -10, -10, -10, -10, -10, -10, -10, -10 }
        };

        public static readonly int[][] WhiteValues = EvaluationFlipper.CalculateWhiteArray(Pattern);
        public static readonly int[][] BlackValues = EvaluationFlipper.CalculateBlackArray(Pattern);

        /// <summary>
        /// Calculates a position values array for the specified player color.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <returns>The position values array for the specified color.</returns>
        public static int[] GetValues(Color color, GamePhase gamePhase)
        {
            return color == Color.White ? WhiteValues[(int)gamePhase] : BlackValues[(int)gamePhase];
        }
    }
}
