using Core.Commons.Positions;
using System;

namespace Core.Boards
{
    public static class BitPositionConverter
    {
        public static ulong ToULong(Position position)
        {
            return 1ul << ((8 - (position.X)) + ((position.Y - 1) * 8));
        }

        public static int ToBitIndex(Position position)
        {
            return (8 - position.X) + ((position.Y - 1) * 8);
        }

        public static Position ToPosition(int bitIndex)
        {
            return new Position(8 - (bitIndex % 8), (bitIndex / 8) + 1);
        }

        public static bool[,] ToBoolArray(ulong bitBoard)
        {
            var boolArray = new bool[8, 8];

            while(bitBoard != 0)
            {
                var lsb = BitOperations.GetLSB(ref bitBoard);
                var bitIndex = BitOperations.GetBitIndex(lsb);
                var position = ToPosition(bitIndex);

                boolArray[position.X - 1, position.Y - 1] = true;
            }

            return boolArray;
        }
    }
}
