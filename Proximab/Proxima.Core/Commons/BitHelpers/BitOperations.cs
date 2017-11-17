using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Performance;
using System.Runtime.CompilerServices;

namespace Proxima.Core.Boards
{
    public static class BitOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetLSB(int value)
        {
            return value & -value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong GetLSB(ulong value)
        {
            var valueWithSign = (long)value;          
            return (ulong)(valueWithSign & -valueWithSign);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopLSB(int value)
        {
            return value & (value - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong PopLSB(ulong value)
        {
            return value & (value - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSingleBit(ulong value)
        {
            value = PopLSB(value);
            return value == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count(ulong value)
        {
            var count = 0;

            while(value != 0)
            {
                value = PopLSB(value);
                count++;
            }

            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBitIndex(ulong value)
        {
            return FastMath.Log2(value);
        }
    }
}
