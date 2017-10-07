using Core.Commons.Positions;
using System;

namespace Core.Boards
{
    internal static class BitPositionConverter
    {
        public static ulong ToULong(Position position)
        {
            return 1ul << ((8 - (position.X)) + ((position.Y - 1) * 8));
        }

        public static Position ToPosition(ulong bitPosition)
        {
            var bitIndex = (int)Math.Log(bitPosition, 2);

            return new Position(8 - (bitIndex % 8), (bitIndex / 8) + 1);
        }

        public static bool[,] ToBoolArray(ulong bitBoard)
        {
            var boolArray = new bool[8, 8];

            while(bitBoard != 0)
            {
                var lsb = BitOperations.GetLSB(ref bitBoard);
                var position = ToPosition(lsb);

                boolArray[position.X - 1, position.Y - 1] = true;
            }

            return boolArray;
        }
    }
}
