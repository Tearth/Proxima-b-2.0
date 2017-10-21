using GUI.App.Source.BoardSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.InputSubsystem;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GUI.App.Source.GameModeSubsystem
{
    internal abstract class GameModeBase
    {
        protected ConsoleManager _consoleManager;
        protected Board _board;

        public GameModeBase(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
            _consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;

            _board = new Board();
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            _board.LoadContent(contentManager);
        }

        public virtual void Input(InputManager inputManager)
        {
            _board.Input(inputManager);
        }

        public virtual void Logic()
        {
            _board.Logic();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _board.Draw(spriteBatch);
        }

        void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;
        }
    }
}
