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
using Proxima.Core.Commons.Pieces;

namespace GUI.App.Source.PromotionSubsystem
{
    /// <summary>
    /// Represents a promotion window (with selectable pieces).
    /// </summary>
    internal class PromotionWindow
    {
        /// <summary>
        /// Gets a value indicating whether window is displayer or not.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// The event triggered when some promotion piece is selected.
        /// </summary>
        public event EventHandler<PromotionSelectedEventArgs> OnPromotionSelection;

        private Texture2D _windowBackground;
        private Texture2D _windowHighlight;

        private Vector2? _highlightPosition;

        private PiecesProvider _piecesProvider;

        private List<PieceType> _predefinedPieceTypes;
        private List<PromotionPiece> _availablePieces;
        private List<PromotionMove> _promotionMoves;

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionWindow"/> class.
        /// </summary>
        /// <param name="piecesProvider">PiecesProvider instance.</param>
        public PromotionWindow(PiecesProvider piecesProvider)
        {
            Active = false;

            _highlightPosition = null;
            _piecesProvider = piecesProvider;

            _predefinedPieceTypes = new List<PieceType>() { PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight };
            _availablePieces = new List<PromotionPiece>();
            _promotionMoves = new List<PromotionMove>();
        }

        /// <summary>
        /// Loads the resources. Must be called before first use of any other class method.
        /// </summary>
        /// <param name="contentManager">Monogame content manager.</param>
        public void LoadContent(ContentManager contentManager)
        {
            _windowBackground = contentManager.Load<Texture2D>("Textures\\PromotionWindowBackground");
            _windowHighlight = contentManager.Load<Texture2D>("Textures\\PromotionWindowHighlight");
        }

        /// <summary>
        /// Processes all events related to mouse and keyboard.
        /// </summary>
        /// <param name="inputManager">InputManager instance.</param>
        public void Input(InputManager inputManager)
        {
            if (!Active)
            {
                return;
            }

            var mousePosition = inputManager.MousePosition;
            _highlightPosition = null;

            if (IsMouseOverPromotionWindow(mousePosition))
            {
                var pieceIndex = GetPieceIndex(mousePosition);
                _highlightPosition = GetHighlightPosition(pieceIndex);

                if (inputManager.LeftMouseButtonJustPressed)
                {
                    var pieceType = _predefinedPieceTypes[pieceIndex];
                    var move = _promotionMoves.First(p => p.PromotionPiece == pieceType);

                    OnPromotionSelection?.Invoke(this, new PromotionSelectedEventArgs(move));
                }
            }
        }

        /// <summary>
        /// Processes all logic related to the promotion window.
        /// </summary>
        public void Logic()
        {
        }

        /// <summary>
        /// Draws promotion window (only if <see cref="Active"/> is true).
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch.</param>
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

        /// <summary>
        /// Shows the promotion window for the specified color.  It must be called only once.
        /// </summary>
        /// <param name="color">The color of promotion pieces.</param>
        /// <param name="promotionMoves">The promotion moves list.</param>
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
        
        /// <summary>
        /// Hides promotion window.
        /// </summary>
        public void Hide()
        {
            _promotionMoves.Clear();
            _availablePieces.Clear();

            Active = false;
        }

        /// <summary>
        /// Draws promotion window background.
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch.</param>
        private void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_windowBackground, Constants.PromotionWindowPosition, Constants.PromotionWindowSize, Color.White);
        }

        /// <summary>
        /// Draws highlight (when mouse is over some promotion piece).
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch.</param>
        private void DrawHighlight(SpriteBatch spriteBatch)
        {
            if (_highlightPosition.HasValue)
            {
                spriteBatch.Draw(_windowHighlight, _highlightPosition.Value, Constants.FieldSize, Color.White);
            }
        }

        /// <summary>
        /// Draws promotion pieces.
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch.</param>
        private void DrawPieces(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _availablePieces.Count; i++)
            {
                var piece = _availablePieces[i];
                var position = GetHighlightPosition(i);

                spriteBatch.Draw(piece.Texture, position, Constants.FieldSize, Color.White);
            }
        }

        /// <summary>
        /// Checks if mouse is over some promotion piece.
        /// </summary>
        /// <param name="mousePosition">The current mouse position.</param>
        /// <returns>True if the mouse is over promotion piece, otherwise false.</returns>
        private bool IsMouseOverPromotionWindow(Point mousePosition)
        {
            return mousePosition.X >= Constants.PromotionWindowPosition.X &&
                   mousePosition.X <= Constants.PromotionWindowPosition.X + Constants.PromotionWindowSize.Width &&
                   mousePosition.Y >= Constants.PromotionWindowPosition.Y &&
                   mousePosition.Y <= Constants.PromotionWindowPosition.Y + Constants.PromotionWindowSize.Height;
        }

        /// <summary>
        /// Calculates piece index basing on current mouse position (from 0 to 3).
        /// </summary>
        /// <param name="mousePosition">The current mouse position.</param>
        /// <returns>The index of the currently selected piece.</returns>
        private int GetPieceIndex(Point mousePosition)
        {
            return (int)Math.Floor((mousePosition.X - Constants.PromotionWindowPosition.X) / Constants.FieldWidthHeight);
        }

        /// <summary>
        /// Calculates the higlight position basing on the promotion piece index.
        /// </summary>
        /// <param name="pieceIndex">The promotion piece index.</param>
        /// <returns>The position of the highlight.</returns>
        private Vector2 GetHighlightPosition(int pieceIndex)
        {
            return Constants.PromotionWindowPosition + new Vector2(pieceIndex * Constants.FieldWidthHeight, 0);
        }
    }
}
