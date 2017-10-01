using GUI.Source.ConsoleSubsystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Source.ConsoleSubsystem.Parser;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Core.Board;
using Microsoft.Xna.Framework;
using Core.Common;
using GUI.Source.InputSubsystem;
using GUI.Source.BoardSubsystem.Selections;
using GUI.Source.Helpers;
using GUI.Source.BoardSubsystem.Axes;
using GUI.Source.BoardSubsystem.Pieces;

namespace GUI.Source.BoardSubsystem
{
    internal class Board
    {
        public event EventHandler<FieldSelectedEventArgs> OnFieldSelection;
        
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

        public void Input(InputManager inputManager)
        {
            var mousePosition = inputManager.GetMousePosition();

            if(inputManager.IsLeftMouseButtonJustPressed())
            {
                _selectionsManager.RemoveAllSelections();
                _selectionsManager.SelectField(mousePosition, _friendlyBoard);
            }

            if(inputManager.IsRightMouseButtonJustPressed())
            {
                _selectionsManager.RemoveAllSelections();
            }
        }

        public void SetBoard(FriendlyBoard friendlyBoard)
        {
            _friendlyBoard = friendlyBoard;
        }

        public void AddExternalSelections(IEnumerable<Position> selections)
        {
            _selectionsManager.AddExternalSelections(selections);
        }

        public void HandleCommand(Command command)
        {

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
            _friendlyBoard.SetPiece(new Position(1, 1), PieceType.WhiteKing);
            _friendlyBoard.SetPiece(new Position(3, 2), PieceType.WhiteKnight);
            _friendlyBoard.SetPiece(new Position(4, 2), PieceType.BlackBishop);
            _friendlyBoard.SetPiece(new Position(6, 6), PieceType.BlackQueen);
            _friendlyBoard.SetPiece(new Position(2, 7), PieceType.WhiteBishop);
            _friendlyBoard.SetPiece(new Position(2, 8), PieceType.WhiteRook);

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

        void SelectionsManager_OnFieldSelection(object sender, FieldSelectedEventArgs e)
        {
            OnFieldSelection?.Invoke(sender, e);
        }
    }
}
