using ContentDefinitions.Colors;
using ContentDefinitions.Commands;
using Core.Common;
using GUI.Source.BoardSubsystem;
using GUI.Source.BoardSubsystem.Selections;
using GUI.Source.ConsoleSubsystem;
using GUI.Source.InputSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GUI
{
    internal class GUICore : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        ConsoleManager _consoleManager;
        InputManager _inputManager;
        Board _board;
        FPSCounter _fpsCounter;
        
        public GUICore(ConsoleManager consoleManager)
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 512;
            _graphics.PreferredBackBufferHeight = 512;

            _inputManager = new InputManager();
            _fpsCounter = new FPSCounter();

            _consoleManager = consoleManager;
            _consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;

            _board = new Board();
            _board.OnFieldSelection += Board_OnFieldSelection;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            _board.LoadContent(Content);
            _fpsCounter.LoadContent(Content);
            _consoleManager.LoadContent(Content);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            Input();

            _fpsCounter.Logic();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _board.Draw(_spriteBatch);
            _fpsCounter.Draw(_spriteBatch);

            _spriteBatch.End();

            _fpsCounter.AddFrame();
            base.Draw(gameTime);
        }

        void Input()
        {
            _inputManager.Logic();
            _board.Input(_inputManager);
            _fpsCounter.Input(_inputManager);
        }

        void ConsoleManager_OnNewCommand(object sender, CommandEventArgs e)
        {
            _consoleManager.HandleCommand(e.Command);
        }

        void Board_OnFieldSelection(object sender, FieldSelectedEventArgs e)
        {
            
        }
    }
}
