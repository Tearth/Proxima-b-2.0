using Core.Commons;
using System;

namespace Core.Boards
{
    internal static class BitPositionConverter
    {
        public static ulong ToULong(Position position)
        {
            return 1ul << ((position.X - 1) + ((position.Y - 1) * 8));
        }

        public static Position ToPosition(ulong bitPosition)
        {
            var bitIndex = (int)Math.Log(bitPosition, 2);

            return new Position((bitIndex % 8) + 1, (bitIndex / 8) + 1);
        }
    }
}
