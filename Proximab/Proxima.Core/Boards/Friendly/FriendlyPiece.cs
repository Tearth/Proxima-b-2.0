using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyPiece
    {
        public Position Position { get; private set; }
        public PieceType Type { get; private set; }
        public Color Color { get; private set; }

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
