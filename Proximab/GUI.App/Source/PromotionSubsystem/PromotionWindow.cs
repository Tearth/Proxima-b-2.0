using System;
using System.Collections.Generic;
using System.Linq;
using GUI.App.Source.BoardSubsystem.Pieces;
using GUI.App.Source.Helpers;
using GUI.App.Source.InputSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Moves;

namespace GUI.App.Source.PromotionSubsystem
{
    internal class PromotionWindow
    {
        public bool Active { get; private set; }

        public event EventHandler<PromotionSelectedEventArgs> OnPromotionSelection;

        private Texture2D _windowBackground;
        private Texture2D _windowHighlight;

        private Vector2? _highlightPosition;

        private PiecesProvider _piecesProvider;

        private List<PieceType> _predefinedPieceTypes;
        private List<PromotionPiece> _availablePieces;
        private List<PromotionMove> _promotionMoves;

        public PromotionWindow(PiecesProvider piecesProvider)
        {
            Active = false;

            _highlightPosition = null;
            _piecesProvider = piecesProvider;

            _predefinedPieceTypes = new List<PieceType>() { PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight };
            _availablePieces = new List<PromotionPiece>();
            _promotionMoves = new List<PromotionMove>();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _windowBackground = contentManager.Load<Texture2D>("Textures\\PromotionWindowBackground");
            _windowHighlight = contentManager.Load<Texture2D>("Textures\\PromotionWindowHighlight");
        }

        public void Input(InputManager inputManager)
        {
            if (!Active)
            {
                return;
            }

            var mousePosition = inputManager.GetMousePosition();
            _highlightPosition = null;

            if (IsMouseOverPromotionWindow(mousePosition))
            {
                var pieceIndex = GetPieceIndex(mousePosition);
                _highlightPosition = GetHighlightPosition(pieceIndex);

                if (inputManager.IsLeftMouseButtonJustPressed())
                {
                    var pieceType = _predefinedPieceTypes[pieceIndex];
                    var move = _promotionMoves.First(p => p.PromotionPiece == pieceType);

                    OnPromotionSelection?.Invoke(this, new PromotionSelectedEventArgs(move));
                }
            }
        }

        public void Logic()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Active)
            {
                return;
            }

            DrawBackground(spriteBatch);
            DrawHighlight(spriteBatch);
            DrawPieces(spriteBatch);
        }

        public void Display(Proxima.Core.Commons.Colors.Color color, IEnumerable<PromotionMove> promotionMoves)
        {
            _promotionMoves.AddRange(promotionMoves);

            foreach (var predefinedPiece in _predefinedPieceTypes)
            {
                var piece = new PromotionPiece(_piecesProvider.GetPieceTexture(color, predefinedPiece), predefinedPiece);
                _availablePieces.Add(piece);
            }

            Active = true;
        }
        
        public void Hide()
        {
            _promotionMoves.Clear();
            _availablePieces.Clear();

            Active = false;
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_windowBackground, Constants.PromotionWindowPosition, Constants.PromotionWindowSize, Color.White);
        }

        private void DrawHighlight(SpriteBatch spriteBatch)
        {
            if (_highlightPosition.HasValue)
            {
                spriteBatch.Draw(_windowHighlight, _highlightPosition.Value, Constants.FieldSize, Color.White);
            }
        }

        private void DrawPieces(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _availablePieces.Count; i++)
            {
                var piece = _availablePieces[i];
                var position = GetHighlightPosition(i);

                spriteBatch.Draw(piece.Texture, position, Constants.FieldSize, Color.White);
            }
        }

        private bool IsMouseOverPromotionWindow(Point mousePosition)
        {
            return mousePosition.X >= Constants.PromotionWindowPosition.X &&
                   mousePosition.X <= Constants.PromotionWindowPosition.X + Constants.PromotionWindowSize.Width &&
                   mousePosition.Y >= Constants.PromotionWindowPosition.Y &&
                   mousePosition.Y <= Constants.PromotionWindowPosition.Y + Constants.PromotionWindowSize.Height;
        }

        private int GetPieceIndex(Point mousePosition)
        {
            return (int)Math.Floor((mousePosition.X - Constants.PromotionWindowPosition.X) / Constants.FieldWidthHeight);
        }

        private Vector2 GetHighlightPosition(int pieceIndex)
        {
            return Constants.PromotionWindowPosition + new Vector2(pieceIndex * Constants.FieldWidthHeight, 0);
        }
    }
}
