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

        public Board()
        {

        }

        public void Init(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void Logic()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            DrawPieces(spriteBatch);
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
            var field1 = _contentManager.Load<Texture2D>("Textures\\Field1");
            var field2 = _contentManager.Load<Texture2D>("Textures\\Field2");

            bool fieldInversion = false;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var position = new Rectangle(x * 64, y * 64, 64, 64);

                    if (fieldInversion)
                        spriteBatch.Draw(field1, position, Color.White);
                    else
                        spriteBatch.Draw(field2, position, Color.White);

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
    }
}
