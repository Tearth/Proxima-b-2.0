using Microsoft.Xna.Framework;

namespace GUI.App.Source.Helpers
{
    internal static class Constants
    {
        //Window
        public static Vector2 WindowSize = new Vector2(548, 548);

        //Field
        public static int FieldWidthHeight { get; } = 64;
        public static Rectangle FieldSize { get; } = new Rectangle(0, 0, FieldWidthHeight, FieldWidthHeight);

        //Board
        public static Vector2 BoardPosition { get; } = new Vector2(18, 18);
        public static int BoardWidthHeight { get; } = FieldWidthHeight * 8;
    }
}
