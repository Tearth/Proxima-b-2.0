using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.InputSubsystem
{
    internal class FPSCounter
    {
        public int FPS { get; private set; }

        int _currentSecond;
        int _currentFPS;
        bool _displayFPS;

        ContentManager _contentManager;

        public FPSCounter()
        {
            FPS = 0;

            _currentSecond = DateTime.Now.Second;
            _currentFPS = 0;
            _displayFPS = true;
        }

        public void Init(ContentManager contentManager)
        {
            _contentManager = contentManager;
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
                var font = _contentManager.Load<SpriteFont>("Fonts\\FPSFont");
                var position = new Vector2(10, 10);
                var text = "FPS: " + FPS;

                spriteBatch.DrawString(font, text, position, Color.DarkRed);
            }
        }
    }
}
