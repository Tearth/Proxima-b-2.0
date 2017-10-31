using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Commons;

namespace GUI.App.Source.PromotionSubsystem
{
    internal class PromotionPiece
    {
        public Texture2D Texture { get; private set; }
        public PieceType Piece { get; private set; }

        public PromotionPiece()
        {

        }

        public PromotionPiece(Texture2D texture, PieceType piece)
        {
            Texture = texture;
            Piece = piece;
        }
    }
}
