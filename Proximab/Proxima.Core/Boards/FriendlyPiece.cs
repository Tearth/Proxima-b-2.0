using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Boards
{
    public class FriendlyPiece
    {
        public PieceType Type { get; set; }
        public Color Color { get; set; }

        public FriendlyPiece() : this(PieceType.Pawn, Color.White)
        {

        }

        public FriendlyPiece(PieceType type, Color color)
        {
            Type = type;
            Color = color;
        }
    }
}
