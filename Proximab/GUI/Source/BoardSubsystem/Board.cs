using Core.Board;
using Core.Common;
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

            _selectionsManager.OnFieldSelection += SelectionsManager_OnFieldSelection;
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
                _selectionsManager.RemoveAllSelections();
                _selectionsManager.SelectField(mousePosition, _friendlyBoard);
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

        void DrawBackground(SpriteBatch spriteBatch)
        {
            bool fieldInversion = false;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var position = new Vector2(x, y) * Constants.FieldWidthHeight;
                    var texture = fieldInversion ? _field1 : _field2;

                    spriteBatch.Draw(texture, position + Constants.BoardPosition, Constants.FieldSize, Color.White);
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

                    spriteBatch.Draw(texture, position + Constants.BoardPosition, Constants.FieldSize, Color.White);
                }
            }
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

        public void AddExternalSelections(IEnumerable<Position> selections)
        {
            _selectionsManager.AddExternalSelections(selections);
        }

        void SelectionsManager_OnFieldSelection(object sender, FieldSelectedEventArgs e)
        {
            OnFieldSelection?.Invoke(sender, e);
        }
    }
}
