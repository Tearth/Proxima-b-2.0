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
        List<Keys> _keyboardJustPressedKeys;
        KeyboardState _keyboardKeysPreviousState;

        bool _leftMouseButtonJustPressed;
        bool _rightMouseButtonJustPressed;

        ButtonState _leftMouseButtonPreviousState;
        ButtonState _rightMouseButtonPreviousState;

        public InputManager()
        {
            _keyboardJustPressedKeys = new List<Keys>();
            _keyboardKeysPreviousState = Keyboard.GetState();

            _leftMouseButtonJustPressed = false;
            _rightMouseButtonJustPressed = false;

            _leftMouseButtonPreviousState = ButtonState.Released;
            _rightMouseButtonPreviousState = ButtonState.Released;
        }

        public void Logic()
        {
            ProcessMouse();
            ProcessKeyboard();
        }

        public bool IsLeftMouseButtonJustPressed()
        {
            if (!_leftMouseButtonJustPressed)
                return false;

            var currentState = _leftMouseButtonJustPressed;
            _leftMouseButtonJustPressed = false;

            return currentState;
        }

        public bool IsRightMouseButtonJustPressed()
        {
            if (!_rightMouseButtonJustPressed)
                return false;

            var currentState = _rightMouseButtonJustPressed;
            _rightMouseButtonJustPressed = false;

            return currentState;
        }

        public bool IsKeyJustPressed(Keys key)
        {
            var pressed = _keyboardJustPressedKeys.Exists(p => p == key);
            if(pressed)
            {
                _keyboardJustPressedKeys.Remove(key);
            }

            return pressed;
        }

        void ProcessMouse()
        {
            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && _leftMouseButtonPreviousState == ButtonState.Released)
            {
                _leftMouseButtonJustPressed = true;
            }

            if (mouseState.RightButton == ButtonState.Pressed && _rightMouseButtonPreviousState == ButtonState.Released)
            {
                _rightMouseButtonJustPressed = true;
            }

            if(mouseState.LeftButton == ButtonState.Released)
            {
                _leftMouseButtonJustPressed = false;
            }

            if (mouseState.RightButton == ButtonState.Released)
            {
                _rightMouseButtonJustPressed = false;
            }

            _leftMouseButtonPreviousState = mouseState.LeftButton;
            _rightMouseButtonPreviousState = mouseState.RightButton;
        }

        void ProcessKeyboard()
        {
            var keyboardState = Keyboard.GetState();
            var pressedKeys = keyboardState.GetPressedKeys();

            foreach (var key in pressedKeys)
            {
                if(_keyboardKeysPreviousState.IsKeyUp(key) && !_keyboardJustPressedKeys.Exists(p => p == key))
                {
                    _keyboardJustPressedKeys.Add(key);
                }
            }

            for(int i = _keyboardJustPressedKeys.Count - 1; i >= 0; i--)
            {
                var key = _keyboardJustPressedKeys[i];
                if(keyboardState.IsKeyUp(key))
                {
                    _keyboardJustPressedKeys.Remove(key);
                }
            }

            _keyboardKeysPreviousState = keyboardState;
        }
    }
}
