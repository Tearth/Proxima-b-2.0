using GUI.App.Source.Helpers;
using GUI.App.Source.InputSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GUI.App.Source.PromotionSubsystem
{
    internal class PromotionWindow
    {
        Texture2D WindowBackground;

        public PromotionWindow()
        {

        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            WindowBackground = contentManager.Load<Texture2D>("Textures\\WindowBackground");
        }

        public virtual void Input(InputManager inputManager)
        {

        }

        public virtual void Logic()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(WindowBackground, Constants.PromotionWindowPosition, Constants.PromotionWindowSize, Color.White);
        }

        public void Display(Proxima.Core.Commons.Colors.Color color)
        {

        }
    }
}
