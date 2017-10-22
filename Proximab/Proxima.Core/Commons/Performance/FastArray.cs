using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Commons.Performance
{
    public class FastArray
    {
        public static int GetIndex(Color color, PieceType pieceType)
        {
            return ((int)color * 6) + (int)pieceType;
        }
    }
}
