using System.Diagnostics.CodeAnalysis;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Mobility
{
    /// <summary>
    /// Represents a set of evaluation parameters for mobility evaluation calculators.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class MobilityValues
    {
        public static readonly int[][] RatioPattern =
        {
            // Regular
            new[] { 1,   1,   1,   1,   1,   1,   1,   1,
                    1,   1,   1,   1,   1,   1,   1,   1,
                    1,   1,   2,   2,   2,   2,   1,   1,
                    1,   1,   2,   4,   4,   2,   1,   1,
                    1,   1,   2,   4,   4,   2,   1,   1,
                    1,   1,   2,   2,   2,   2,   1,   1,
                    1,   1,   1,   1,   1,   1,   1,   1,
                    1,   1,   1,   1,   1,   1,   1,   1 },

             // End
            new[] { 1,   1,   1,   1,   1,   1,   1,   1,
                    1,   1,   1,   1,   1,   1,   1,   1,
                    1,   1,   1,   1,   1,   1,   1,   1,
                    1,   1,   1,   1,   1,   1,   1,   1,
                    1,   1,   1,   1,   1,   1,   1,   1,
                    1,   1,   1,   1,   1,   1,   1,   1,
                    1,   1,   1,   1,   1,   1,   1,   1,
                    1,   1,   1,   1,   1,   1,   1,   1 }
        };

        public static readonly int[][] WhiteRatio = EvaluationFlipper.CalculateWhiteArray(RatioPattern);
        public static readonly int[][] BlackRatio = EvaluationFlipper.CalculateBlackArray(RatioPattern);

        /// <summary>
        /// Calculates a mobility ratio based on the specified color.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <returns>The mobility ratio array for the specified color.</returns>
        public static int[] GetRatio(Color color, GamePhase gamePhase)
        {
            return color == Color.White ? WhiteRatio[(int)gamePhase] : BlackRatio[(int)gamePhase];
        }
    }
}
