using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.MoveGenerators.MagicBitboards
{
    /// <summary>
    /// Represents a set of constants for magic bitboards.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class MagicConstants
    {
        public const int RookMaxMovesBits = 12;
        public const int RookMaxMovesPerField = 1 << RookMaxMovesBits;

        public const int BishopMaxMovesBits = 9;
        public const int BishopMaxMovesPerField = 1 << BishopMaxMovesBits;
    }
}
