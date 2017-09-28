using ContentDefinitions.Colors;
using ContentDefinitions.Commands;
using GUI.Source.ConsoleSubsystem;
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
        
        public GUICore(ConsoleManager consoleManager)
        {
            _graphics = new GraphicsDeviceManager(this);

            _consoleManager = consoleManager;
            _consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            var commandDefinitions = Content.Load<CommandDefinitionsContainer>("XML\\CommandDefinitions");
            var colorDefinitions = Content.Load<ColorDefinitionsContainer>("XML\\ColorDefinitions");

            _consoleManager.SetCommandDefinitions(commandDefinitions, colorDefinitions);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

        private void ConsoleManager_OnNewCommand(object sender, CommandEventArgs e)
        {
            _consoleManager.HandleCommand(e.Command);
        }
    }
}
