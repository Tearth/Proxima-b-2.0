using GUI.App.Source.BoardSubsystem.Pieces;
using GUI.App.Source.Helpers;
using GUI.App.Source.InputSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Commons;
using System.Collections.Generic;

namespace GUI.App.Source.PromotionSubsystem
{
    internal class PromotionWindow
    {
        Texture2D _windowBackground;

        PiecesProvider _piecesProvider;
        List<Texture2D> _availablePieces;

        public PromotionWindow(PiecesProvider piecesProvider)
        {
            _piecesProvider = piecesProvider;
            _availablePieces = new List<Texture2D>();
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            _windowBackground = contentManager.Load<Texture2D>("Textures\\WindowBackground");
        }

        public virtual void Input(InputManager inputManager)
        {

        }

        public virtual void Logic()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_windowBackground, Constants.PromotionWindowPosition, Constants.PromotionWindowSize, Color.White);

            for(int i=0; i<_availablePieces.Count; i++)
            {
                var piece = _availablePieces[i];
                var position = Constants.PromotionWindowPosition + new Vector2(Constants.FieldWidthHeight * i, 0);

                spriteBatch.Draw(piece, position, Constants.FieldSize, Color.White);
            }
        }

        public void Display(Proxima.Core.Commons.Colors.Color color)
        {
            _availablePieces.Clear();

            _availablePieces.Add(_piecesProvider.GetPieceTexture(color, PieceType.Queen));
            _availablePieces.Add(_piecesProvider.GetPieceTexture(color, PieceType.Rook));
            _availablePieces.Add(_piecesProvider.GetPieceTexture(color, PieceType.Bishop));
            _availablePieces.Add(_piecesProvider.GetPieceTexture(color, PieceType.Knight));
        }
    }
}
