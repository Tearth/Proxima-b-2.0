using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyPiece
    {
        public Position Position { get; set; }
        public PieceType Type { get; set; }
        public Color Color { get; set; }

        public FriendlyPiece()
        {

        }

        public FriendlyPiece(Position position, PieceType type, Color color)
        {
            Position = position;
            Type = type;
            Color = color;
        }
    }
}
