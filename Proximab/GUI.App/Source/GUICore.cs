using System;
using GUI.App.Source.CommandsSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.GameModeSubsystem;
using GUI.App.Source.GameModeSubsystem.Editor;
using GUI.App.Source.Helpers;
using GUI.App.Source.InputSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GUI.App.Source
{
    /// <summary>
    /// The main class of project. Represents a set of methods for drawing and processing logic.
    /// It's the bridge between the whole GUI and Monogame library.
    /// </summary>
    internal class GUICore : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private ModeBase _mode;
        private ModeFactory _modeFactory;

        private ConsoleManager _consoleManager;
        private CommandsManager _commandsManager;
        private InputManager _inputManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GUICore"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager instance.</param>
        /// <param name="commandsManager">The commands manager instance.</param>
        public GUICore(ConsoleManager consoleManager, CommandsManager commandsManager)
        {
            _consoleManager = consoleManager;
            _commandsManager = commandsManager;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int)Constants.WindowSize.X,
                PreferredBackBufferHeight = (int)Constants.WindowSize.Y
            };

            _modeFactory = new ModeFactory(_consoleManager, _commandsManager);
            _mode = _modeFactory.Create(ModeType.Editor);

            _inputManager = new InputManager();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            SetCommandHandlers();
        }

        /// <summary>
        /// Initializes Monogame library.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }
        
        /// <summary>
        /// Loads the resources. Must be called before first use of any other class method.
        /// </summary>
        protected override void LoadContent()
        {
            _consoleManager.LoadContent(Content);
            _mode.LoadContent(Content);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        /// <summary>
        /// Releases resources.
        /// </summary>
        protected override void UnloadContent()
        {
        }
        
        /// <summary>
        /// Updates all game objects with specified delta time.
        /// </summary>
        /// <param name="gameTime">Time parameters.</param>
        protected override void Update(GameTime gameTime)
        {
            Input();

            _mode.Logic();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws all game objects.
        /// </summary>
        /// <param name="gameTime">Time parameters.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _mode.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Processes all input events.
        /// </summary>
        private void Input()
        {
            _inputManager.Logic();
            _mode.Input(_inputManager);
        }

        /// <summary>
        /// Adds all command handlers from current class to the commands manager.
        /// </summary>
        private void SetCommandHandlers()
        {
            _commandsManager.AddCommandHandler(CommandType.Mode, CommandGroup.GUICore, ChangeMode);
        }

        /// <summary>
        /// Changes mode to the specified one.
        /// </summary>
        /// <param name="command">The Mode command.</param>
        private void ChangeMode(Command command)
        {
            var modeNameArgument = command.GetArgument<string>(0);

            var modeNameParseResult = Enum.TryParse(modeNameArgument, true, out ModeType modeType);
            if (!modeNameParseResult)
            {
                _consoleManager.WriteLine($"$rInvalid mode type ($R{modeNameArgument}$r)");
                return;
            }

            _commandsManager.RemoveCommandHandlers(CommandGroup.GameMode);
            _mode = _modeFactory.Create(modeType);
            _mode.LoadContent(Content);
        }
    }
}
