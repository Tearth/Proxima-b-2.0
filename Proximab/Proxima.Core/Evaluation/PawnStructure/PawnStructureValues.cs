using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.Evaluation.PawnStructure
{
    /// <summary>
    /// Represents a set of evaluation parameters for pawn structure evaluation calculators.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class PawnStructureValues
    {
        public static readonly int[] DoubledPawnsRatio =
        {
            -30,  // Regular
            -50    // End
        };

        public static readonly int[] IsolatedPawnsRatio =
        {
            -40,  // Regular
            -15    // End
        };

        public static readonly int[] PawnChainRatio =
        {
            15,  // Regular
            5    // End
        };
    }
}
