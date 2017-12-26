using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Commons.Pieces;

namespace GUI.App.PromotionSubsystem
{
    /// <summary>
    /// Represents information about promotion piece.
    /// </summary>
    public class PromotionPiece
    {
        /// <summary>
        /// Gets the piece texture.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets the piece type.
        /// </summary>
        public PieceType Piece { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionPiece"/> class.
        /// </summary>
        /// <param name="texture">The piece texture.</param>
        /// <param name="piece">The piece type.</param>
        public PromotionPiece(Texture2D texture, PieceType piece)
        {
            Texture = texture;
            Piece = piece;
        }
    }
}
