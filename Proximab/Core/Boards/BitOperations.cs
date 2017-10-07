using System;

namespace Core.Boards
{
    public static class BitOperations
    {
        public static ulong GetLSB(ref ulong value)
        {
            var lsb = (ulong)((long)value & -((long)value));
            value &= value - 1;

            return lsb;
        }

        public static int GetBitIndex(ulong bit)
        {
            return (int)Math.Log(bit, 2);
        }
    }
}
