using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Performance;
using System.Runtime.CompilerServices;

namespace Proxima.Core.Boards
{
    public static class BitOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetLSB(ref int value)
        {
            var valueWithSign = value;
            value &= value - 1;

            return valueWithSign & -valueWithSign;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong GetLSB(ref ulong value)
        {
            var valueWithSign = (long)value;
            value &= value - 1;
            
            return (ulong)(valueWithSign & -valueWithSign);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSingleBit(ulong value)
        {
            GetLSB(ref value);
            return value == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count(ulong value)
        {
            var count = 0;

            while(value != 0)
            {
                GetLSB(ref value);
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
