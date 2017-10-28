using GUI.App.Source.BoardSubsystem;
using GUI.App.Source.BoardSubsystem.Pieces;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.InputSubsystem;
using GUI.App.Source.PromotionSubsystem;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GUI.App.Source.GameModeSubsystem
{
    internal abstract class GameModeBase
    {
        protected ConsoleManager _consoleManager;
        protected PiecesProvider _piecesProvider;

        protected Board _board;
        protected PromotionWindow _promotionWindow;

        public GameModeBase(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
            _consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;

            _piecesProvider = new PiecesProvider();

            _board = new Board(_piecesProvider);
            _promotionWindow = new PromotionWindow();
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            _piecesProvider.LoadContent(contentManager);
            _board.LoadContent(contentManager);
            _promotionWindow.LoadContent(contentManager);
        }

        public virtual void Input(InputManager inputManager)
        {
            _board.Input(inputManager);
            _promotionWindow.Input(inputManager);
        }

        public virtual void Logic()
        {
            _board.Logic();
            _promotionWindow.Logic();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _board.Draw(spriteBatch);
            _promotionWindow.Draw(spriteBatch);
        }

        void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;
        }
    }
}
