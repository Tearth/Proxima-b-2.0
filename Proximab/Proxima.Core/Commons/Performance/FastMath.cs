using System.Runtime.CompilerServices;

namespace Proxima.Core.Commons.Performance
{
    public static class FastMath
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2(ulong value)
        {
            var bits = 0;

            if (value > 0xfffffffful)
            {
                value >>= 32;
                bits = 0x20;
            }

            if (value > 0xfffful)
            {
                value >>= 16;
                bits |= 0x10;
            }

            if (value > 0xfful)
            {
                value >>= 8;
                bits |= 0x8;
            }

            if (value > 0xful)
            {
                value >>= 4;
                bits |= 0x4;
            }

            if (value > 0x3ul)
            {
                value >>= 2;
                bits |= 0x2;
            }

            if (value > 0x1ul)
            {
                bits |= 0x1;
            }

            return bits;
        }
    }
}
