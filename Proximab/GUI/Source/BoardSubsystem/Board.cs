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

namespace GUI.Source.BoardSubsystem
{
    internal class Board
    {
        ContentManager _contentManager;
        FriendlyBoard _friendlyBoard;

        Texture2D _field1;
        Texture2D _field2;
        Texture2D _selection;

        public Board()
        {

        }

        public void Init(ContentManager contentManager)
        {
            _contentManager = contentManager;

            _field1 = _contentManager.Load<Texture2D>("Textures\\Field1");
            _field2 = _contentManager.Load<Texture2D>("Textures\\Field2");
            _selection = _contentManager.Load<Texture2D>("Textures\\Selection");
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

        public void SetBoard(FriendlyBoard friendlyBoard)
        {
            _friendlyBoard = friendlyBoard;
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
                    var position = new Rectangle(x * 64, y * 64, 64, 64);
                    var texture = fieldInversion ? _field1 : _field2;

                    spriteBatch.Draw(texture, position, Color.White);
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
        }
    }
}
