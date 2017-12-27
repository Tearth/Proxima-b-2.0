namespace Proxima.Core.Evaluation.PawnStructure
{
    /// <summary>
    /// Represents a set of evaluation parameters for pawn structure evaluation calculators.
    /// </summary>
    public static class PawnStructureValues
    {
        public static readonly int[] DoubledPawnsRatio = new int[]
        {
            -10,  // Regular
            -5    // End
        };

        public static readonly int[] IsolatedPawnsRatio = new int[]
        {
            -10,  // Regular
            -5    // End
        };

        public static readonly int[] PawnChainRatio = new int[]
        {
            10,  // Regular
            5    // End
        };
    }
}
