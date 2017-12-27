using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GUI.App.InputSubsystem
{
    /// <summary>
    /// Represents a set of methods to manage application input.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// Gets a value indicating whether right mouse button is just pressed.
        /// </summary>
        public bool RightMouseButtonJustPressed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether left mouse button is just pressed.
        /// </summary>
        public bool LeftMouseButtonJustPressed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether right mouse button is just released.
        /// </summary>
        public bool RightMouseButtonJustReleased { get; private set; }

        /// <summary>
        /// Gets a value indicating whether left mouse button is just released.
        /// </summary>
        public bool LeftMouseButtonJustReleased { get; private set; }

        /// <summary>
        /// Gets a value indicating whether left mouse button is pressed.
        /// </summary>
        public bool IsLeftMouseButtonPressed => Mouse.GetState().LeftButton == ButtonState.Pressed;

        /// <summary>
        /// Gets a value indicating whether right mouse button is pressed.
        /// </summary>
        public bool IsRightMouseButtonPressed => Mouse.GetState().RightButton == ButtonState.Pressed;

        /// <summary>
        /// Gets the current mouse position.
        /// </summary>
        public Point MousePosition => Mouse.GetState().Position;

        private List<Keys> _keyboardJustPressedKeys;
        private KeyboardState _keyboardKeysPreviousState;

        private ButtonState _leftMouseButtonPreviousState;
        private ButtonState _rightMouseButtonPreviousState;

        private Point _lastMouseMovePosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager"/> class.
        /// </summary>
        public InputManager()
        {
            _keyboardJustPressedKeys = new List<Keys>();
            _keyboardKeysPreviousState = Keyboard.GetState();

            LeftMouseButtonJustPressed = false;
            RightMouseButtonJustPressed = false;

            LeftMouseButtonJustReleased = false;
            RightMouseButtonJustReleased = false;

            _leftMouseButtonPreviousState = ButtonState.Released;
            _rightMouseButtonPreviousState = ButtonState.Released;

            _lastMouseMovePosition = MousePosition;
        }

        /// <summary>
        /// Processes all logic related to the input.
        /// </summary>
        public void Logic()
        {
            LeftMouseButtonJustPressed = false;
            RightMouseButtonJustPressed = false;
            LeftMouseButtonJustReleased = false;
            RightMouseButtonJustReleased = false;

            ProcessMouse();
            ProcessKeyboard();
        }

        /// <summary>
        /// Checks if the specified key is just pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the specified key is pressed, otherwise false.</returns>
        public bool IsKeyJustPressed(Keys key)
        {
            var pressed = _keyboardJustPressedKeys.Exists(p => p == key);
            if (pressed)
            {
                _keyboardJustPressedKeys.Remove(key);
            }

            return pressed;
        }
        
        /// <summary>
        /// Calculates the mouse move delta (before previous and current position).
        /// </summary>
        /// <returns>The move delta</returns>
        public Vector2 GetMouseMoveDelta()
        {
            var currentState = _lastMouseMovePosition;
            _lastMouseMovePosition = MousePosition;

            return new Vector2(_lastMouseMovePosition.X - currentState.X, _lastMouseMovePosition.Y - currentState.Y);
        }

        /// <summary>
        /// Processes mouse events.
        /// </summary>
        private void ProcessMouse()
        {
            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && _leftMouseButtonPreviousState == ButtonState.Released)
            {
                LeftMouseButtonJustPressed = true;
            }

            if (mouseState.RightButton == ButtonState.Pressed && _rightMouseButtonPreviousState == ButtonState.Released)
            {
                RightMouseButtonJustPressed = true;
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                if (_leftMouseButtonPreviousState == ButtonState.Pressed)
                {
                    LeftMouseButtonJustReleased = true;
                }
            }

            if (mouseState.RightButton == ButtonState.Released)
            {
                if (_rightMouseButtonPreviousState == ButtonState.Pressed)
                {
                    RightMouseButtonJustReleased = true;
                }
            }

            _leftMouseButtonPreviousState = mouseState.LeftButton;
            _rightMouseButtonPreviousState = mouseState.RightButton;
        }

        /// <summary>
        /// Processes keyboard events.
        /// </summary>
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

            for (var i = _keyboardJustPressedKeys.Count - 1; i >= 0; i--)
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
