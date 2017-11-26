using System.Runtime.CompilerServices;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Boards
{
    /// <summary>
    /// Represents a set of bit operations.
    /// </summary>
    public static class BitOperations
    {
        /// <summary>
        /// Calculates a least significant bit.
        /// </summary>
        /// <param name="value">The value to calculate.</param>
        /// <returns>The least significant bit if value is greater than 0, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetLSB(int value)
        {
            return value & -value;
        }

        /// <summary>
        /// Calculates a least significant bit.
        /// </summary>
        /// <param name="value">The value to calculate.</param>
        /// <returns>The least significant bit if value is greater than 0, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong GetLSB(ulong value)
        {
            var valueWithSign = (long)value;
            return (ulong)(valueWithSign & -valueWithSign);
        }

        /// <summary>
        /// Pops (removes) a least significant bit.
        /// </summary>
        /// <param name="value">The value to calculate.</param>
        /// <returns>The value without the least significant bit.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopLSB(int value)
        {
            return value & (value - 1);
        }

        /// <summary>
        /// Pops (removes) a least significant bit.
        /// </summary>
        /// <param name="value">The value to calculate.</param>
        /// <returns>The value without the least significant bit.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong PopLSB(ulong value)
        {
            return value & (value - 1);
        }

        /// <summary>
        /// Checks if the specified value has more than one set bit. 
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the specified value has only one set bit, otherwise false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSingleBit(ulong value)
        {
            value = PopLSB(value);
            return value == 0;
        }

        /// <summary>
        /// Counts set bits in the specified value.
        /// </summary>
        /// <param name="value">The value to calculate.</param>
        /// <returns>The number of set bits.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count(ulong value)
        {
            var count = 0;

            while (value != 0)
            {
                value = PopLSB(value);
                count++;
            }

            return count;
        }

        /// <summary>
        /// Calculates a bit index (where LSB has index 0 and MSB has index 64).
        /// </summary>
        /// <param name="value">The value to calculate (must be only one bit set, otherwise result will be strange).</param>
        /// <returns>The bit index.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBitIndex(ulong value)
        {
            return FastMath.Log2(value);
        }
    }
}
