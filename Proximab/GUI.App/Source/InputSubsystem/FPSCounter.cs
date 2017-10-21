using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GUI.App.Source.InputSubsystem
{
    internal class FPSCounter
    {
        public int FPS { get; private set; }
        public Vector2 FPSPosition { get; set; }
        public Color FPSColor { get; set; }

        int _currentSecond;
        int _currentFPS;
        bool _displayFPS;

        SpriteFont _fpsFont;

        public FPSCounter()
        {
            FPS = 0;

            FPSPosition = new Vector2(5, 5);
            FPSColor = Color.DarkGreen;

            _currentSecond = DateTime.Now.Second;
            _currentFPS = 0;
            _displayFPS = false;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _fpsFont = contentManager.Load<SpriteFont>("Fonts\\FPSFont");
        }

        public void Logic()
        {
            var second = DateTime.Now.Second;

            if(_currentSecond != second)
            {
                FPS = _currentFPS;
                _currentFPS = 0;
            }

            _currentSecond = second;
        }

        public void Input(InputManager inputManager)
        {
            if(inputManager.IsKeyJustPressed(Keys.Tab))
            {
                _displayFPS = !_displayFPS;
            }
        }

        public void AddFrame()
        {
            _currentFPS++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(_displayFPS)
            {
                var text = "FPS: " + FPS;

                spriteBatch.DrawString(_fpsFont, text, FPSPosition, FPSColor);
            }
        }
    }
}
