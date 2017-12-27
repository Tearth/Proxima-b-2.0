namespace Proxima.Core.Evaluation.Material
{
    /// <summary>
    /// Represents a set of evaluation parameters for material evaluation calculators.
    /// </summary>
    public static class MaterialValues
    {
        public static readonly int[] PieceValues =
        {
            100,    // Pawn
            320,    // Knight
            330,    // Bishop
            500,    // Rook
            1000,   // Queen
            10000   // King
        };
    }
}
