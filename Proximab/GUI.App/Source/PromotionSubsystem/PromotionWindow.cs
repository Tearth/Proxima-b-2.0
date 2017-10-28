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
            spriteBatch.Draw(WindowBackground, new Vector2(0, 0), new Rectangle(0, 0, 192, 64), Color.White);
        }
    }
}
