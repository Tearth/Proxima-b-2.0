using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GUI.App.Source.InputSubsystem
{
    internal class InputManager
    {
        List<Keys> _keyboardJustPressedKeys;
        KeyboardState _keyboardKeysPreviousState;

        bool _leftMouseButtonJustPressed;
        bool _rightMouseButtonJustPressed;

        bool _leftMouseButtonJustReleased;
        bool _rightMouseButtonJustReleased;

        ButtonState _leftMouseButtonPreviousState;
        ButtonState _rightMouseButtonPreviousState;

        Point _lastMouseMovePosition;

        public InputManager()
        {
            _keyboardJustPressedKeys = new List<Keys>();
            _keyboardKeysPreviousState = Keyboard.GetState();

            _leftMouseButtonJustPressed = false;
            _rightMouseButtonJustPressed = false;

            _leftMouseButtonJustReleased = false;
            _rightMouseButtonJustReleased = false;

            _leftMouseButtonPreviousState = ButtonState.Released;
            _rightMouseButtonPreviousState = ButtonState.Released;

            _lastMouseMovePosition = GetMousePosition();
        }

        public void Logic()
        {
            ProcessMouse();
            ProcessKeyboard();
        }

        public bool IsLeftMouseButtonJustPressed()
        {
            var currentState = _leftMouseButtonJustPressed;
            _leftMouseButtonJustPressed = false;

            return currentState;
        }

        public bool IsRightMouseButtonJustPressed()
        {
            var currentState = _rightMouseButtonJustPressed;
            _rightMouseButtonJustPressed = false;

            return currentState;
        }

        public bool IsLeftMouseButtonJustReleased()
        {
            var currentState = _leftMouseButtonJustReleased;
            _leftMouseButtonJustReleased = false;

            return currentState;
        }

        public bool IsRightMouseButtonJustReleased()
        {
            var currentState = _rightMouseButtonJustReleased;
            _rightMouseButtonJustReleased = false;

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

        public bool IsLeftMouseButtonPressed()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }

        public bool IsRightMouseButtonPressed()
        {
            return Mouse.GetState().RightButton == ButtonState.Pressed;
        }

        public Point GetMousePosition()
        {
            return Mouse.GetState().Position;
        }

        public Vector2 GetMouseMoveDelta()
        {
            var currentState = _lastMouseMovePosition;
            _lastMouseMovePosition = GetMousePosition();

            return new Vector2(_lastMouseMovePosition.X - currentState.X, _lastMouseMovePosition.Y - currentState.Y);
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

                if(_leftMouseButtonPreviousState == ButtonState.Pressed)
                {
                    _leftMouseButtonJustReleased = true;
                }
            }

            if (mouseState.RightButton == ButtonState.Released)
            {
                _rightMouseButtonJustPressed = false;

                if (_rightMouseButtonPreviousState == ButtonState.Pressed)
                {
                    _rightMouseButtonJustReleased = true;
                }
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
