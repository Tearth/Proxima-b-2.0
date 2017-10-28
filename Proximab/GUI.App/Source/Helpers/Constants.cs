using Microsoft.Xna.Framework;

namespace GUI.App.Source.Helpers
{
    internal static class Constants
    {
        //Window
        public static readonly Vector2 WindowSize = new Vector2(548, 548);

        //Field
        public static readonly int FieldWidthHeight = 64;
        public static readonly Rectangle FieldSize = new Rectangle(0, 0, FieldWidthHeight, FieldWidthHeight);

        //Board
        public static readonly Vector2 BoardPosition = new Vector2(18, 18);
        public static readonly int BoardWidthHeight = FieldWidthHeight * 8;

        //Promotion window
        public static readonly Rectangle PromotionWindowSize = new Rectangle(0, 0, 256, 64);
        public static readonly Vector2 PromotionWindowPosition = (WindowSize - new Vector2(PromotionWindowSize.Width, PromotionWindowSize.Height)) / 2;
    }
}
