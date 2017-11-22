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
    /// Represents a board drawed on window. It's the bridge between the GUI project ang Monogame library.
    /// </summary>
    internal class VisualBoard
    {
        /// <summary>
        /// The event triggered when some field on the board is selected.
        /// </summary>
        public event EventHandler<FieldSelectedEventArgs> OnFieldSelection;

        /// <summary>
        /// The event triggered whem some piece on the board is moved.
        /// </summary>
        public event EventHandler<PieceMovedEventArgs> OnPieceMove;

        private FriendlyBoard _friendlyBoard;
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
            _friendlyBoard = new FriendlyBoard();

            _selectionsManager = new SelectionsManager();
            _axesManager = new AxesManager();

            _piecesProvider = piecesProvider;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _field1 = contentManager.Load<Texture2D>("Textures\\Field1");
            _field2 = contentManager.Load<Texture2D>("Textures\\Field2");

            _selectionsManager.LoadContent(contentManager);
            _axesManager.LoadContent(contentManager);
        }

        public void Input(InputManager inputManager)
        {
            var mousePosition = inputManager.GetMousePosition();

            if (inputManager.IsLeftMouseButtonJustPressed())
            {
                var previousSelection = _selectionsManager.GetInternalSelection();
                _selectionsManager.RemoveAllSelections();

                var selectedFieldPosition = _selectionsManager.SelectField(mousePosition);
                var selectedPiece = _friendlyBoard.GetPiece(selectedFieldPosition);

                if (previousSelection == null)
                {
                    ProcessLeftButtonPressWithoutPreviousSelection(selectedFieldPosition, selectedPiece);
                }
                else
                {
                    ProcessLeftButtonPressWithPreviousSelection(previousSelection, selectedFieldPosition, selectedPiece);
                }
            }

            if (inputManager.IsRightMouseButtonJustPressed())
            {
                _selectionsManager.RemoveAllSelections();
            }
        }

        public void Logic()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            DrawPieces(spriteBatch);

            _selectionsManager.Draw(spriteBatch);
            _axesManager.Draw(spriteBatch);
        }

        public FriendlyBoard GetFriendlyBoard()
        {
            return _friendlyBoard;
        }

        public void SetFriendlyBoard(FriendlyBoard friendlyBoard)
        {
            _friendlyBoard = friendlyBoard;
        }

        public void AddPiece(Position position, FriendlyPiece piece)
        {
            _friendlyBoard.SetPiece(piece);
        }

        public void AddExternalSelections(List<Position> selections)
        {
            _selectionsManager.AddExternalSelections(selections);
        }

        private void ProcessLeftButtonPressWithPreviousSelection(Selection previousSelection, Position selectedPosition, FriendlyPiece selectedPieceType)
        {
            var previousSelectedPiece = _friendlyBoard.GetPiece(previousSelection.Position);

            if (previousSelectedPiece == null)
            {
                OnFieldSelection?.Invoke(this, new FieldSelectedEventArgs(selectedPosition, selectedPieceType));
            }
            else if (previousSelectedPiece != null && selectedPieceType == null)
            {
                var from = previousSelection.Position;
                var to = selectedPosition;

                OnPieceMove(this, new PieceMovedEventArgs(previousSelectedPiece, from, to));

                _selectionsManager.RemoveAllSelections();
            }
        }

        private void ProcessLeftButtonPressWithoutPreviousSelection(Position selectedPosition, FriendlyPiece selectedPieceType)
        {
            OnFieldSelection?.Invoke(this, new FieldSelectedEventArgs(selectedPosition, selectedPieceType));
        }

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

        private void DrawPieces(SpriteBatch spriteBatch)
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    var boardPosition = new Position(x, y);
                    var piece = _friendlyBoard.GetPiece(boardPosition);

                    if (piece == null)
                        continue;

                    var position = new Microsoft.Xna.Framework.Vector2(boardPosition.X - 1, 8 - boardPosition.Y) * Constants.FieldWidthHeight;
                    var texture = _piecesProvider.GetPieceTexture(piece.Color, piece.Type);

                    spriteBatch.Draw(texture, position + Constants.BoardPosition, Constants.FieldSize, Microsoft.Xna.Framework.Color.White);
                }
            }
        }
    }
}