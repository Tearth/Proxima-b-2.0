using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.InputSubsystem
{
    internal class InputManager
    {
        bool _leftMouseButtonJustPressed;
        bool _rightMouseButtonJustPressed;

        ButtonState _leftMouseButtonPreviousState;
        ButtonState _rightMouseButtonPreviousState;

        public InputManager()
        {
            _leftMouseButtonJustPressed = false;
            _rightMouseButtonJustPressed = false;

            _leftMouseButtonPreviousState = ButtonState.Released;
            _rightMouseButtonPreviousState = ButtonState.Released;
        }

        public void Logic()
        {
            var mouseState = Mouse.GetState();

            if(mouseState.LeftButton == ButtonState.Pressed && _leftMouseButtonPreviousState == ButtonState.Released)
            {
                _leftMouseButtonJustPressed = true;
            }

            if (mouseState.RightButton == ButtonState.Pressed && _rightMouseButtonPreviousState == ButtonState.Released)
            {
                _rightMouseButtonJustPressed = true;
            }

            _leftMouseButtonPreviousState = mouseState.LeftButton;
            _rightMouseButtonPreviousState = mouseState.RightButton;
        }

        public bool IsLeftMouseButtonPressed()
        {
            if (!_leftMouseButtonJustPressed)
                return false;

            var currentState = _leftMouseButtonJustPressed;
            _leftMouseButtonJustPressed = false;

            return currentState;
        }

        public bool IsRightMouseButtonPressed()
        {
            if (!_rightMouseButtonJustPressed)
                return false;

            var currentState = _rightMouseButtonJustPressed;
            _rightMouseButtonJustPressed = false;

            return currentState;
        }
    }
}
