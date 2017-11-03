using Proxima.Core.Commons.Positions;
using System.Runtime.CompilerServices;

namespace Proxima.Core.Boards
{
    public static class BitPositionConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToBitIndex(Position position)
        {
            return (8 - position.X) + ((position.Y - 1) << 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Position ToPosition(int bitIndex)
        {
            return new Position(8 - (bitIndex % 8), (bitIndex >> 3) + 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToULong(Position position)
        {
            return 1ul << ToBitIndex(position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToULong(bool[,] bitBoard)
        {
            ulong value = 0;
            ulong currentBit = 1;

            for(int y=0; y<8; y++)
            {
                for(int x=0; x<8; x++)
                {
                    if(bitBoard[x, y])
                    {
                        var position = new Position(x + 1, y + 1);
                        var positionValue = ToULong(position);

                        value |= positionValue;
                    }

                    currentBit <<= 1;
                }
            }

            return value;
        }
    }
}
