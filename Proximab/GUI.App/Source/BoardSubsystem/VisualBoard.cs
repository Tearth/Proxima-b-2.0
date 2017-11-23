using System;
using System.Collections.Generic;
using GUI.App.Source.BoardSubsystem.Axes;
using GUI.App.Source.BoardSubsystem.Pieces;
using GUI.App.Source.BoardSubsystem.Selections;
using GUI.App.Source.Helpers;
using GUI.App.Source.InputSubsystem;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Positions;

namespace GUI.App.Source.BoardSubsystem
{
    /// <summary>
    /// Represents a board drawed on window.
    /// </summary>
    internal class VisualBoard
    {
        /// <summary>
        /// The event triggered when some field on the board is selected.
        /// </summary>
        public event EventHandler<FieldSelectedEventArgs> OnFieldSelection;

        /// <summary>
        /// The event triggered when some piece on the board is moved.
        /// </summary>
        public event EventHandler<PieceMovedEventArgs> OnPieceMove;

        /// <summary>
        /// Gets or sets the friendly board.
        /// </summary>
        public FriendlyBoard FriendlyBoard { get; set; }

        private SelectionsManager _selectionsManager;
        private AxesManager _axesManager;
        private PiecesProvider _piecesProvider;

        private Texture2D _field1;
        private Texture2D _field2;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualBoard"/> class.
        /// </summary>
        /// <param name="piecesProvider">The pieces provider.</param>
        public VisualBoard(PiecesProvider piecesProvider)
        {
            FriendlyBoard = new FriendlyBoard();

            _selectionsManager = new SelectionsManager();
            _axesManager = new AxesManager();

            _piecesProvider = piecesProvider;
        }

        /// <summary>
        /// Loads resources. Must be called before first use.
        /// </summary>
        /// <param name="contentManager">Monogame content manager</param>
        public void LoadContent(ContentManager contentManager)
        {
            _field1 = contentManager.Load<Texture2D>("Textures\\Field1");
            _field2 = contentManager.Load<Texture2D>("Textures\\Field2");

            _selectionsManager.LoadContent(contentManager);
            _axesManager.LoadContent(contentManager);
        }

        /// <summary>
        /// Processes all events related to mouse and keyboard.
        /// </summary>
        /// <param name="inputManager">InputManager instance.</param>
        public void Input(InputManager inputManager)
        {
            var mousePosition = inputManager.MousePosition;

            if (inputManager.LeftMouseButtonJustPressed)
            {
                var previousSelection = _selectionsManager.GetInternalSelection();
                _selectionsManager.RemoveAllSelections();

                var selectedFieldPosition = _selectionsManager.SelectField(mousePosition);
                var selectedPiece = FriendlyBoard.GetPiece(selectedFieldPosition);

                if (previousSelection == null)
                {
                    ProcessLeftButtonPressWithoutPreviousSelection(selectedFieldPosition, selectedPiece);
                }
                else
                {
                    ProcessLeftButtonPressWithPreviousSelection(previousSelection, selectedFieldPosition, selectedPiece);
                }
            }

            if (inputManager.RightMouseButtonJustPressed)
            {
                _selectionsManager.RemoveAllSelections();
            }
        }

        /// <summary>
        /// Processes all logic related to the visual board.
        /// </summary>
        public void Logic()
        {
        }

        /// <summary>
        /// Draws the board with axes and selections.
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            DrawPieces(spriteBatch);

            _selectionsManager.Draw(spriteBatch);
            _axesManager.Draw(spriteBatch);
        }

        /// <summary>
        /// Adds list of external selections.
        /// </summary>
        /// <param name="selections">The list of selections.</param>
        public void AddExternalSelections(List<Position> selections)
        {
            _selectionsManager.AddExternalSelections(selections);
        }

        /// <summary>
        /// Processes the left button press when there is another selection present.
        /// </summary>
        /// <param name="previousSelection">Previous selection.</param>
        /// <param name="selectedPosition">Selection position on board.</param>
        /// <param name="selectedPiece">Selected piece (null if the selected field is empty).</param>
        private void ProcessLeftButtonPressWithPreviousSelection(Selection previousSelection, Position selectedPosition, FriendlyPiece selectedPiece)
        {
            var previousSelectedPiece = FriendlyBoard.GetPiece(previousSelection.Position);

            if (previousSelectedPiece == null)
            {
                OnFieldSelection?.Invoke(this, new FieldSelectedEventArgs(selectedPosition, selectedPiece));
            }
            else if (previousSelectedPiece != null && selectedPiece == null)
            {
                var from = previousSelection.Position;
                var to = selectedPosition;

                OnPieceMove(this, new PieceMovedEventArgs(previousSelectedPiece, from, to));

                _selectionsManager.RemoveAllSelections();
            }
        }

        /// <summary>
        /// Processes the left button press when there is no previous selection.
        /// </summary>
        /// <param name="selectionPosition">Selection position on board.</param>
        /// <param name="selectedPieceType">Selected piece type (null if the selected field empty)/</param>
        private void ProcessLeftButtonPressWithoutPreviousSelection(Position selectionPosition, FriendlyPiece selectedPieceType)
        {
            OnFieldSelection?.Invoke(this, new FieldSelectedEventArgs(selectionPosition, selectedPieceType));
        }

        /// <summary>
        /// Draws background (white and black tiles).
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch.</param>
        private void DrawBackground(SpriteBatch spriteBatch)
        {
            bool fieldInversion = false;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var position = new Microsoft.Xna.Framework.Vector2(x, y) * Constants.FieldWidthHeight;
                    var texture = fieldInversion ? _field1 : _field2;

                    spriteBatch.Draw(texture, position + Constants.BoardPosition, Constants.FieldSize, Microsoft.Xna.Framework.Color.White);
                    fieldInversion = !fieldInversion;
                }

                fieldInversion = !fieldInversion;
            }
        }

        /// <summary>
        /// Draws the board.
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch.</param>
        private void DrawPieces(SpriteBatch spriteBatch)
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    var boardPosition = new Position(x, y);
                    var piece = FriendlyBoard.GetPiece(boardPosition);

                    if (piece == null)
                    {
                        continue;
                    }

                    var position = new Microsoft.Xna.Framework.Vector2(boardPosition.X - 1, 8 - boardPosition.Y) * Constants.FieldWidthHeight;
                    var texture = _piecesProvider.GetPieceTexture(piece.Color, piece.Type);

                    spriteBatch.Draw(texture, position + Constants.BoardPosition, Constants.FieldSize, Microsoft.Xna.Framework.Color.White);
                }
            }
        }
    }
}