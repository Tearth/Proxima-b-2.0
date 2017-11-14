using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using System.Runtime.CompilerServices;

namespace Proxima.Core.Commons.Performance
{
    public class FastArray
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetPieceIndex(Color color, PieceType pieceType)
        {
            return ((int)color * 6) + (int)pieceType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetZobristPieceIndex(Color color, PieceType pieceType, int field)
        {
            return ((((int)color * 6) + (int)pieceType) << 6) + field;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCastlingIndex(Color color, CastlingType castlingType)
        {
            return ((int)color << 1) + (int)castlingType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSlideIndex(int position, byte rank)
        {
            return (rank << 3) + (8 - position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetPositionValueIndex(Color color, GamePhase gamePhase, int pieceIndex)
        {
            var arrayPieceIndex = color == Color.White ? 64 - pieceIndex - 1: pieceIndex;
            return ((int)gamePhase << 6) + arrayPieceIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool[,] Merge(bool[,] a, bool[,] b)
        {
            var xSize = a.GetLength(0);
            var ySize = a.GetLength(1);

            var mergedArray = new bool[xSize, ySize];

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    mergedArray[x, y] = a[x, y] || b[x, y];
                }
            }

            return mergedArray;
        }
    }
}
