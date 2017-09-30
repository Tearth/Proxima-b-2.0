﻿using GUI.Source.ConsoleSubsystem;
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

namespace GUI.Source.BoardSubsystem
{
    internal class Board
    {
        readonly int FieldWidthHeight = 64;
        readonly Rectangle FieldSize = new Rectangle(0, 0, 64, 64);
        readonly Vector2 BoardPosition = new Vector2(0, 0);
        
        FriendlyBoard _friendlyBoard;

        Texture2D _field1;
        Texture2D _field2;
        Texture2D _selection;

        List<Position> _selections;

        public Board()
        {
            _selections = new List<Position>();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _field1 = contentManager.Load<Texture2D>("Textures\\Field1");
            _field2 = contentManager.Load<Texture2D>("Textures\\Field2");
            _selection = contentManager.Load<Texture2D>("Textures\\Selection");
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
                var position = new Vector2(selection.X - 1, 8 - selection.Y) * FieldWidthHeight;
                spriteBatch.Draw(_selection, position + BoardPosition, FieldSize, Color.White);
            }
        }

        void SelectField(InputManager inputManager)
        {
            var mousePosition = inputManager.GetMousePosition();

            var fieldX = (mousePosition.X / FieldWidthHeight) + (int)BoardPosition.X + 1;
            var fieldY = 8 - (mousePosition.Y / FieldWidthHeight) + (int)BoardPosition.Y;

            fieldX = Math.Min(8, fieldX);
            fieldY = Math.Min(8, fieldY);

            fieldX = Math.Max(1, fieldX);
            fieldY = Math.Max(1, fieldY);

            _selections.Add(new Position(fieldX, fieldY));
        }

        void RemoveAllSelections()
        {
            _selections.Clear();
        }
    }
}