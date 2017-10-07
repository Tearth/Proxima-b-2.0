using GUI.Source.ConsoleSubsystem;
using GUI.Source.GameModeSubsystem;
using GUI.Source.GameModeSubsystem.Editor;
using GUI.Source.Helpers;
using GUI.Source.InputSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GUI
{
    internal class GUICore : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        ConsoleManager _consoleManager;
        InputManager _inputManager;
        FPSCounter _fpsCounter;

        GameModeBase _gameMode;

        public GUICore(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int)Constants.WindowSize.X;
            _graphics.PreferredBackBufferHeight = (int)Constants.WindowSize.Y;

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

        void Input()
        {
            _inputManager.Logic();

            _gameMode.Input(_inputManager);
            _fpsCounter.Input(_inputManager);
        }
    }
}
