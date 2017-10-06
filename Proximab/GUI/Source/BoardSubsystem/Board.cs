using Core.Boards;
using Core.Commons;
using GUI.Source.BoardSubsystem.Axes;
using GUI.Source.BoardSubsystem.Pieces;
using GUI.Source.BoardSubsystem.Selections;
using GUI.Source.Helpers;
using GUI.Source.InputSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GUI.Source.BoardSubsystem
{
    internal class Board
    {
        public event EventHandler<FieldSelectedEventArgs> OnFieldSelection;
        public event EventHandler<PieceMovedEventArgs> OnPieceMove;
        
        FriendlyBoard _friendlyBoard;
        SelectionsManager _selectionsManager;
        AxesManager _axesManager;
        PiecesProvider _piecesProvider;

        Texture2D _field1;
        Texture2D _field2;

        public Board()
        {
            _friendlyBoard = new FriendlyBoard();
            _selectionsManager = new SelectionsManager();
            _axesManager = new AxesManager();
            _piecesProvider = new PiecesProvider();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _field1 = contentManager.Load<Texture2D>("Textures\\Field1");
            _field2 = contentManager.Load<Texture2D>("Textures\\Field2");

            _selectionsManager.LoadContent(contentManager);
            _axesManager.LoadContent(contentManager);
            _piecesProvider.LoadContent(contentManager);
        }

        public void Input(InputManager inputManager)
        {
            var mousePosition = inputManager.GetMousePosition();

            if (inputManager.IsLeftMouseButtonJustPressed())
            {
                var previousSelection = _selectionsManager.GetInternalSelection();
                _selectionsManager.RemoveAllSelections();

                var selectedFieldPosition = _selectionsManager.SelectField(mousePosition, _friendlyBoard);
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

        public void AddPiece(Position position, PieceType pieceType)
        {
            _friendlyBoard.SetPiece(position, pieceType);
        }

        public void MovePiece(Position from, Position to)
        {
            var piece = _friendlyBoard.GetPiece(from);

            _friendlyBoard.SetPiece(to, piece);
            _friendlyBoard.SetPiece(from, PieceType.None);
        }

        public void AddExternalSelections(List<Position> selections)
        {
            _selectionsManager.AddExternalSelections(selections);
        }

        public void AddExternalSelections(bool[,] boolArray)
        {
            var selections = new List<Position>();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (!boolArray[x, y])
                        continue;

                    selections.Add(new Position(x + 1, y + 1));
                }
            }

            _selectionsManager.AddExternalSelections(selections);
        }

        void ProcessLeftButtonPressWithPreviousSelection(Selection previousSelection, Position selectedPosition, PieceType selectedPieceType)
        {
            var previousSelectedPiece = _friendlyBoard.GetPiece(previousSelection.Position);

            if(previousSelectedPiece == PieceType.None)
            {
                OnFieldSelection?.Invoke(this, new FieldSelectedEventArgs(selectedPosition, selectedPieceType));
            }
            else if (previousSelectedPiece != PieceType.None && selectedPieceType == PieceType.None)
            {
                var from = previousSelection.Position;
                var to = selectedPosition;

                OnPieceMove(this, new PieceMovedEventArgs(previousSelectedPiece, from, to));

                _selectionsManager.RemoveAllSelections();
            }
        }

        void ProcessLeftButtonPressWithoutPreviousSelection(Position selectedPosition, PieceType selectedPieceType)
        {
            OnFieldSelection?.Invoke(this, new FieldSelectedEventArgs(selectedPosition, selectedPieceType));
        }

        void DrawBackground(SpriteBatch spriteBatch)
        {
            bool fieldInversion = false;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var position = new Vector2(x, y) * Constants.FieldWidthHeight;
                    var texture = fieldInversion ? _field1 : _field2;

                    spriteBatch.Draw(texture, position + Constants.BoardPosition, Constants.FieldSize, Microsoft.Xna.Framework.Color.White);
                    fieldInversion = !fieldInversion;
                }

                fieldInversion = !fieldInversion;
            }
        }

        void DrawPieces(SpriteBatch spriteBatch)
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    var boardPosition = new Position(x, y);
                    var piece = _friendlyBoard.GetPiece(boardPosition);

                    if (piece == PieceType.None)
                        continue;

                    var position = new Vector2(boardPosition.X - 1, 8 - boardPosition.Y) * Constants.FieldWidthHeight;
                    var texture = _piecesProvider.GetPieceTexture(piece);

                    spriteBatch.Draw(texture, position + Constants.BoardPosition, Constants.FieldSize, Microsoft.Xna.Framework.Color.White);
                }
            }
        }
    }
}