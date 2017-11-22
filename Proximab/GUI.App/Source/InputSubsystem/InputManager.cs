using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GUI.App.Source.InputSubsystem
{
    internal class InputManager
    {
        private List<Keys> _keyboardJustPressedKeys;
        private KeyboardState _keyboardKeysPreviousState;

        private bool _leftMouseButtonJustPressed;
        private bool _rightMouseButtonJustPressed;

        private bool _leftMouseButtonJustReleased;
        private bool _rightMouseButtonJustReleased;

        private ButtonState _leftMouseButtonPreviousState;
        private ButtonState _rightMouseButtonPreviousState;

        private Point _lastMouseMovePosition;

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
            _leftMouseButtonJustPressed = false;
            _rightMouseButtonJustPressed = false;
            _leftMouseButtonJustReleased = false;
            _rightMouseButtonJustReleased = false;

            ProcessMouse();
            ProcessKeyboard();
        }

        public bool IsLeftMouseButtonJustPressed()
        {
            return _leftMouseButtonJustPressed;
        }

        public bool IsRightMouseButtonJustPressed()
        {
            return _rightMouseButtonJustPressed;
        }

        public bool IsLeftMouseButtonJustReleased()
        {
            return _leftMouseButtonJustReleased;
        }

        public bool IsRightMouseButtonJustReleased()
        {
            return _rightMouseButtonJustReleased;
        }

        public bool IsKeyJustPressed(Keys key)
        {
            var pressed = _keyboardJustPressedKeys.Exists(p => p == key);
            if (pressed)
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

        private void ProcessMouse()
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

            if (mouseState.LeftButton == ButtonState.Released)
            {
                if (_leftMouseButtonPreviousState == ButtonState.Pressed)
                {
                    _leftMouseButtonJustReleased = true;
                }
            }

            if (mouseState.RightButton == ButtonState.Released)
            {
                if (_rightMouseButtonPreviousState == ButtonState.Pressed)
                {
                    _rightMouseButtonJustReleased = true;
                }
            }

            _leftMouseButtonPreviousState = mouseState.LeftButton;
            _rightMouseButtonPreviousState = mouseState.RightButton;
        }

        private void ProcessKeyboard()
        {
            var keyboardState = Keyboard.GetState();
            var pressedKeys = keyboardState.GetPressedKeys();

            foreach (var key in pressedKeys)
            {
                if (_keyboardKeysPreviousState.IsKeyUp(key) && !_keyboardJustPressedKeys.Exists(p => p == key))
                {
                    _keyboardJustPressedKeys.Add(key);
                }
            }

            for (int i = _keyboardJustPressedKeys.Count - 1; i >= 0; i--)
            {
                var key = _keyboardJustPressedKeys[i];
                if (keyboardState.IsKeyUp(key))
                {
                    _keyboardJustPressedKeys.Remove(key);
                }
            }

            _keyboardKeysPreviousState = keyboardState;
        }
    }
}
