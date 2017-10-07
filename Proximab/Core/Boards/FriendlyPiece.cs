using Core.Commons;
using Core.Commons.Colors;

namespace Core.Boards
{
    public class FriendlyPiece
    {
        public PieceType Type { get; set; }
        public Color Color { get; set; }

        public FriendlyPiece() : this(PieceType.None, Color.None)
        {

        }

        public FriendlyPiece(PieceType type, Color color)
        {
            Type = type;
            Color = color;
        }
    }
}
