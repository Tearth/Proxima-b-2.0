using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.GameModeSubsystem;
using GUI.App.Source.GameModeSubsystem.Editor;
using GUI.App.Source.Helpers;
using GUI.App.Source.InputSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GUI.App.Source
{
    internal class GUICore : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private ConsoleManager _consoleManager;
        private InputManager _inputManager;
        private FPSCounter _fpsCounter;

        private GameModeBase _gameMode;

        public GUICore(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int)Constants.WindowSize.X,
                PreferredBackBufferHeight = (int)Constants.WindowSize.Y
            };

            _inputManager = new InputManager();
            _fpsCounter = new FPSCounter();

            _gameMode = new EditorGameMode(consoleManager);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            _fpsCounter.LoadContent(Content);
            _consoleManager.LoadContent(Content);
            _gameMode.LoadContent(Content);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            Input();

            _gameMode.Logic();
            _fpsCounter.Logic();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _gameMode.Draw(_spriteBatch);
            _fpsCounter.Draw(_spriteBatch);

            _spriteBatch.End();

            _fpsCounter.AddFrame();
            base.Draw(gameTime);
        }

        private void Input()
        {
            _inputManager.Logic();

            _gameMode.Input(_inputManager);
            _fpsCounter.Input(_inputManager);
        }
    }
}
