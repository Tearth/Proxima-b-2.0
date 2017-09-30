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

namespace GUI.Source.BoardSubsystem
{
    internal class Board
    {
        public event EventHandler<FieldSelectedEventArgs> OnFieldSelection;

        readonly int FieldWidthHeight = 64;
        readonly Rectangle FieldSize = new Rectangle(0, 0, 64, 64);
        readonly Vector2 BoardPosition = new Vector2(0, 0);
        
        FriendlyBoard _friendlyBoard;

        Texture2D _field1;
        Texture2D _field2;
        Texture2D _internalSelection;
        Texture2D _externalSelection;

        List<Selection> _selections;

        public Board()
        {
            _friendlyBoard = new FriendlyBoard();
            _selections = new List<Selection>();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _field1 = contentManager.Load<Texture2D>("Textures\\Field1");
            _field2 = contentManager.Load<Texture2D>("Textures\\Field2");
            _internalSelection = contentManager.Load<Texture2D>("Textures\\InternalSelection");
            _externalSelection = contentManager.Load<Texture2D>("Textures\\ExternalSelection");
        }

        public void Logic()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            DrawPieces(spriteBatch);
            DrawSelections(spriteBatch);
        }

        public void Input(InputManager inputManager)
        {
            if(inputManager.IsLeftMouseButtonJustPressed())
            {
                RemoveAllSelections();
                SelectField(inputManager);
            }

            if(inputManager.IsRightMouseButtonJustPressed())
            {
                RemoveAllSelections();
            }
        }

        public void SetBoard(FriendlyBoard friendlyBoard)
        {
            _friendlyBoard = friendlyBoard;
        }

        public void AddExternalSelections(IEnumerable<Position> selections)
        {
            foreach(var position in selections)
            {
                if(!_selections.Exists(p => p.Type == SelectionType.External && p.Position == position))
                {
                    _selections.Add(new Selection(position, SelectionType.External));
                }
            }
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
                    var position = new Vector2(x, y) * FieldWidthHeight;
                    var texture = fieldInversion ? _field1 : _field2;

                    spriteBatch.Draw(texture, position + BoardPosition, FieldSize, Color.White);
                    fieldInversion = !fieldInversion;
                }

                fieldInversion = !fieldInversion;
            }
        }

        void DrawPieces(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {

                }
            }
        }

        void DrawSelections(SpriteBatch spriteBatch)
        {
            foreach(var selection in _selections)
            {
                Texture2D texture = null;
                switch(selection.Type)
                {
                    case SelectionType.Internal: { texture = _internalSelection; break; }
                    case SelectionType.External: { texture = _externalSelection; break; }
                }

                var position = new Vector2(selection.Position.X - 1, 8 - selection.Position.Y) * FieldWidthHeight;

                spriteBatch.Draw(texture, position + BoardPosition, FieldSize, Color.White);
            }
        }

        void SelectField(InputManager inputManager)
        {
            var mousePosition = inputManager.GetMousePosition();

            var fieldX = (int)((mousePosition.X - BoardPosition.X) / FieldWidthHeight) + 1;
            var fieldY = 8 - (int)((mousePosition.Y - BoardPosition.Y) / FieldWidthHeight);

            fieldX = Math.Min(8, fieldX);
            fieldY = Math.Min(8, fieldY);

            fieldX = Math.Max(1, fieldX);
            fieldY = Math.Max(1, fieldY);

            var position = new Position(fieldX, fieldY);
            _selections.Add(new Selection(position, SelectionType.Internal));

            var selectedPiece = _friendlyBoard.GetPieceAtPosition(position);
            OnFieldSelection?.Invoke(this, new FieldSelectedEventArgs(position, selectedPiece));
        }

        void RemoveAllSelections()
        {
            _selections.Clear();
        }
    }
}
