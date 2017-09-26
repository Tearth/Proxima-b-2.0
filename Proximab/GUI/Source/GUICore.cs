using GUI.Source.ConsoleSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

            Content.RootDirectory = "Assets";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            


            base.Draw(gameTime);
        }
    }
}
