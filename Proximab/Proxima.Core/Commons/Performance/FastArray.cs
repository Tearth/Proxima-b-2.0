using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Commons.Performance
{
    public class FastArray
    {
        public static int GetIndex(Color color, PieceType pieceType)
        {
            return ((int)color * 6) + (int)pieceType;
        }

        public static int GetIndex(Color color, CastlingType castlingType)
        {
            return ((int)color << 1) + (int)castlingType;
        }
    }
}
