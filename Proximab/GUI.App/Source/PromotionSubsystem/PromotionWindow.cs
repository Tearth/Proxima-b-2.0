using GUI.App.Source.BoardSubsystem.Pieces;
using GUI.App.Source.Helpers;
using GUI.App.Source.InputSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Commons;
using System;
using System.Collections.Generic;

namespace GUI.App.Source.PromotionSubsystem
{
    internal class PromotionWindow
    {
        Texture2D _windowBackground;
        Texture2D _windowHighlight;

        Vector2? _highlightPosition;

        PiecesProvider _piecesProvider;
        List<Texture2D> _availablePieces;

        public PromotionWindow(PiecesProvider piecesProvider)
        {
            _piecesProvider = piecesProvider;
            _availablePieces = new List<Texture2D>();

            _highlightPosition = null;
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            _windowBackground = contentManager.Load<Texture2D>("Textures\\PromotionWindowBackground");
            _windowHighlight = contentManager.Load<Texture2D>("Textures\\PromotionWindowHighlight");
        }

        public virtual void Input(InputManager inputManager)
        {
            var mousePosition = inputManager.GetMousePosition();
            _highlightPosition = null;

            if (mousePosition.Y >= Constants.PromotionWindowPosition.Y &&
                mousePosition.Y <= Constants.PromotionWindowPosition.Y + Constants.PromotionWindowSize.Height)
            {
                var pieceIndex = (int)Math.Floor((mousePosition.X - Constants.PromotionWindowPosition.X) / Constants.FieldWidthHeight);
                if(pieceIndex >= 0 && pieceIndex <= 3)
                {
                    _highlightPosition = Constants.PromotionWindowPosition + new Vector2(pieceIndex * Constants.FieldWidthHeight, 0);
                }
            }
        }

        public virtual void Logic()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_windowBackground, Constants.PromotionWindowPosition, Constants.PromotionWindowSize, Color.White);

            if(_highlightPosition.HasValue)
            {
                spriteBatch.Draw(_windowHighlight, _highlightPosition.Value, Constants.FieldSize, Color.White);
            }

            for (int i=0; i<_availablePieces.Count; i++)
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
