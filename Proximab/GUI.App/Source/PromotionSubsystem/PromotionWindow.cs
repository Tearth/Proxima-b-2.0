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
        public bool Active { get; private set; }

        Texture2D _windowBackground;
        Texture2D _windowHighlight;

        Vector2? _highlightPosition;

        PiecesProvider _piecesProvider;
        List<Texture2D> _availablePieces;

        public PromotionWindow(PiecesProvider piecesProvider)
        {
            Active = false;

            _piecesProvider = piecesProvider;
            _availablePieces = new List<Texture2D>();

            _highlightPosition = null;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _windowBackground = contentManager.Load<Texture2D>("Textures\\PromotionWindowBackground");
            _windowHighlight = contentManager.Load<Texture2D>("Textures\\PromotionWindowHighlight");
        }

        public void Input(InputManager inputManager)
        {
            if (!Active || inputManager.GetMouseMoveDelta().Length() == 0)
                return;

            var mousePosition = inputManager.GetMousePosition();
            _highlightPosition = null;

            if (IsMouseOverPromotionWindow(mousePosition))
            {
                var pieceIndex = GetPieceIndex(mousePosition);
                _highlightPosition = GetHighlightPosition(pieceIndex);
            }
        }

        public void Logic()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Active)
                return;

            DrawBackground(spriteBatch);
            DrawHighlight(spriteBatch);
            DrawPieces(spriteBatch);
        }

        void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_windowBackground, Constants.PromotionWindowPosition, Constants.PromotionWindowSize, Color.White);
        }

        void DrawHighlight(SpriteBatch spriteBatch)
        {
            if (_highlightPosition.HasValue)
            {
                spriteBatch.Draw(_windowHighlight, _highlightPosition.Value, Constants.FieldSize, Color.White);
            }
        }

        void DrawPieces(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _availablePieces.Count; i++)
            {
                var piece = _availablePieces[i];
                var position = GetHighlightPosition(i);

                spriteBatch.Draw(piece, position, Constants.FieldSize, Color.White);
            }
        }

        public void Display(Proxima.Core.Commons.Colors.Color color)
        {
            _availablePieces.Add(_piecesProvider.GetPieceTexture(color, PieceType.Queen));
            _availablePieces.Add(_piecesProvider.GetPieceTexture(color, PieceType.Rook));
            _availablePieces.Add(_piecesProvider.GetPieceTexture(color, PieceType.Bishop));
            _availablePieces.Add(_piecesProvider.GetPieceTexture(color, PieceType.Knight));

            Active = true;
        }

        public void Hide()
        {
            _availablePieces.Clear();
            Active = false;
        }

        bool IsMouseOverPromotionWindow(Point mousePosition)
        {
            return mousePosition.X >= Constants.PromotionWindowPosition.X &&
                   mousePosition.X <= Constants.PromotionWindowPosition.X + Constants.PromotionWindowSize.Width &&
                   mousePosition.Y >= Constants.PromotionWindowPosition.Y &&
                   mousePosition.Y <= Constants.PromotionWindowPosition.Y + Constants.PromotionWindowSize.Height;
        }

        int GetPieceIndex(Point mousePosition)
        {
            return (int)Math.Floor((mousePosition.X - Constants.PromotionWindowPosition.X) / Constants.FieldWidthHeight);
        }

        Vector2 GetHighlightPosition(int pieceIndex)
        {
            return Constants.PromotionWindowPosition + new Vector2(pieceIndex * Constants.FieldWidthHeight, 0);
        }
    }
}
