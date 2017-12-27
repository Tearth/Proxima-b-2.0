using Proxima.Core.Commons.BitHelpers;

namespace Proxima.Core.MoveGenerators.PatternGenerators
{
    /// <summary>
    /// Represents a set of methods to generate knight patterns.
    /// </summary>
    public class KnightPatternGenerator
    {
        /// <summary>
        /// Generates knight patterns for every field.
        /// </summary>
        /// <returns>The 64-element array with patterns.</returns>
        public ulong[] Generate()
        {
            var predefinedMoves = new ulong[64];
            var bitPosition = 1ul;

            for (var i = 0; i < 64; i++)
            {
                predefinedMoves[i] = GetPattern(bitPosition);
                bitPosition <<= 1;
            }

            return predefinedMoves;
        }

        /// <summary>
        /// Calculates pattern for the specified field.
        /// </summary>
        /// <param name="fieldBitboard">The field bitboard.</param>
        /// <returns>The pattern for the specified field.</returns>
        private ulong GetPattern(ulong fieldBitboard)
        {
            return ((fieldBitboard & ~BitConstants.AFile) << 17) |
                   ((fieldBitboard & ~BitConstants.HFile) << 15) |
                   ((fieldBitboard & ~BitConstants.AFile & ~BitConstants.BFile) << 10) |
                   ((fieldBitboard & ~BitConstants.GFile & ~BitConstants.HFile) << 6) |
                   ((fieldBitboard & ~BitConstants.AFile & ~BitConstants.BFile) >> 6) |
                   ((fieldBitboard & ~BitConstants.GFile & ~BitConstants.HFile) >> 10) |
                   ((fieldBitboard & ~BitConstants.AFile) >> 15) |
                   ((fieldBitboard & ~BitConstants.HFile) >> 17);
        }
    }
}
