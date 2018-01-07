using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.Evaluation.Material
{
    /// <summary>
    /// Represents a set of evaluation parameters for material evaluation calculators.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class MaterialValues
    {
        public static readonly int[] PieceValues =
        {
            100,    // Pawn
            320,    // Knight
            330,    // Bishop
            500,    // Rook
            1200,   // Queen
            10000   // King
        };
    }
}
