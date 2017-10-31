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

        protected VisualBoard _visualBoard;
        protected PromotionWindow _promotionWindow;

        public GameModeBase(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
            _consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;

            _piecesProvider = new PiecesProvider();

            _visualBoard = new VisualBoard(_piecesProvider);
            _promotionWindow = new PromotionWindow(_piecesProvider);
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            _piecesProvider.LoadContent(contentManager);
            _visualBoard.LoadContent(contentManager);
            _promotionWindow.LoadContent(contentManager);
        }

        public virtual void Input(InputManager inputManager)
        {
            if(!_promotionWindow.Active)
            {
                _visualBoard.Input(inputManager);
            }

            _promotionWindow.Input(inputManager);
        }

        public virtual void Logic()
        {
            _visualBoard.Logic();
            _promotionWindow.Logic();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _visualBoard.Draw(spriteBatch);
            _promotionWindow.Draw(spriteBatch);
        }

        void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;
        }
    }
}
