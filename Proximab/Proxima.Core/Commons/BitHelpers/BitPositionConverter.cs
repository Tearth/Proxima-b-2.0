using System.Runtime.CompilerServices;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Commons.BitHelpers
{
    /// <summary>
    /// Represents a set of methods to convert between <see cref="Position"/> and bit representation.
    /// </summary>
    public static class BitPositionConverter
    {
        /// <summary>
        /// Converts a position into the bitboard (bit on the specified position is set).
        /// </summary>
        /// <param name="position">The position to calculate.</param>
        /// <returns>The bitboard with one bit set.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToBitIndex(Position position)
        {
            return 8 - position.X + ((position.Y - 1) << 3);
        }

        /// <summary>
        /// Converts a bitboard into the position.
        /// </summary>
        /// <param name="bitIndex">The bitboard to convert (must be only one bit set).</param>
        /// <returns>The position.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Position ToPosition(int bitIndex)
        {
            return new Position(8 - (bitIndex % 8), (bitIndex >> 3) + 1);
        }

        /// <summary>
        /// Converts a position into the bitboard.
        /// </summary>
        /// <param name="position">The position to convert.</param>
        /// <returns>The bitboard with one bit set at the specified position.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToULong(Position position)
        {
            return 1ul << ToBitIndex(position);
        }
    }
}
