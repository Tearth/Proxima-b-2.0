using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Commons.Performance
{
    public class FastArray
    {
        public static int GetPieceIndex(Color color, PieceType pieceType)
        {
            return ((int)color * 6) + (int)pieceType;
        }

        public static int GetCastlingIndex(Color color, CastlingType castlingType)
        {
            return ((int)color << 1) + (int)castlingType;
        }

        public static int GetSlideIndex(int position, byte rank)
        {
            return (rank << 3) + (8 - position);
        }
    }
}
