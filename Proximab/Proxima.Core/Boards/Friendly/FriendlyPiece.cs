using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    /// <summary>
    /// Represents a piece in the user-friendly way.
    /// </summary>
    public class FriendlyPiece
    {
        /// <summary>
        /// Gets the piece position.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// Gets the piece type.
        /// </summary>
        public PieceType Type { get; private set; }

        /// <summary>
        /// Gets the piece color.
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyPiece"/> class.
        /// </summary>
        public FriendlyPiece()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyPiece"/> class.
        /// </summary>
        /// <param name="position">The piece position.</param>
        /// <param name="type">The piece type.</param>
        /// <param name="color">The piece color.</param>
        public FriendlyPiece(Position position, PieceType type, Color color)
        {
            Position = position;
            Type = type;
            Color = color;
        }
    }
}
