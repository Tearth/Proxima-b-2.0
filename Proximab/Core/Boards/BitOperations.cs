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

        public static ulong FlipVertical(ulong bitBoard)
        {
            return ((bitBoard << 56)) |
                   ((bitBoard << 40) & BitConstants.GRank) |
                   ((bitBoard << 24) & BitConstants.FRank) |
                   ((bitBoard << 8) & BitConstants.ERank) |
                   ((bitBoard >> 8) & BitConstants.DRank) |
                   ((bitBoard >> 24) & BitConstants.CRank) |
                   ((bitBoard >> 40) & BitConstants.BRank) |
                   ((bitBoard >> 56));
        }

        public static ulong FlipHorizontal(ulong bitBoard)
        {
            ulong k1 = 0x5555555555555555;
            ulong k2 = 0x3333333333333333;
            ulong k4 = 0x0f0f0f0f0f0f0f0f;

            bitBoard = ((bitBoard >> 1) & k1) | ((bitBoard & k1) << 1);
            bitBoard = ((bitBoard >> 2) & k2) | ((bitBoard & k2) << 2);
            bitBoard = ((bitBoard >> 4) & k4) | ((bitBoard & k4) << 4);
            return bitBoard;
        }

        public static ulong FlipDiagonalA1H8(ulong bitBoard)
        {
            ulong flippedBitBoard = 0;

            ulong k1 = 0x5500550055005500;
            ulong k2 = 0x3333000033330000;
            ulong k4 = 0x0f0f0f0f00000000;

            flippedBitBoard = k4 & (bitBoard ^ (bitBoard << 28));
            bitBoard ^= flippedBitBoard ^ (flippedBitBoard >> 28);

            flippedBitBoard = k2 & (bitBoard ^ (bitBoard << 14));
            bitBoard ^= flippedBitBoard ^ (flippedBitBoard >> 14);

            flippedBitBoard = k1 & (bitBoard ^ (bitBoard << 7));
            bitBoard ^= flippedBitBoard ^ (flippedBitBoard >> 7);

            return bitBoard;
        }

        public static ulong FlipDiagonalA8H1(ulong bitBoard)
        {
            ulong flippedBitBoard = 0;

            ulong k1 = 0xaa00aa00aa00aa00;
            ulong k2 = 0xcccc0000cccc0000;
            ulong k4 = 0xf0f0f0f00f0f0f0f;

            flippedBitBoard = bitBoard ^ (bitBoard << 36);
            bitBoard ^= k4 & (flippedBitBoard ^ (bitBoard >> 36));

            flippedBitBoard = k2 & (bitBoard ^ (bitBoard << 18));
            bitBoard ^= flippedBitBoard ^ (flippedBitBoard >> 18);

            flippedBitBoard = k1 & (bitBoard ^ (bitBoard << 9));
            bitBoard ^= flippedBitBoard ^ (flippedBitBoard >> 9);

            return bitBoard;
        }

        public static ulong RotateRight(ulong bitBoard)
        {
            return FlipDiagonalA1H8(FlipVertical(bitBoard));
        }

        public static ulong RotateLeft(ulong bitBoard)
        {
            return FlipVertical(FlipDiagonalA1H8(bitBoard));
        }
    }
}
