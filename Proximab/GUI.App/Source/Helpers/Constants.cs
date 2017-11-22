using Microsoft.Xna.Framework;

namespace GUI.App.Source.Helpers
{
    /// <summary>
    /// Represents a set of constant fields for Draw functions.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The size of the window (must be enough big to hold a whole board with axes).
        /// </summary>
        public static readonly Vector2 WindowSize = new Vector2(548, 548);

        /// <summary>
        /// The size of the single field in pixels (rectangle, so width = height).
        /// </summary>
        public static readonly int FieldWidthHeight = 64;

        /// <summary>
        /// The size of the single field in Rectangle object.
        /// </summary>
        public static readonly Rectangle FieldSize = new Rectangle(0, 0, FieldWidthHeight, FieldWidthHeight);

        /// <summary>
        /// The position of the board in pixels.
        /// </summary>
        public static readonly Vector2 BoardPosition = new Vector2(18, 18);

        /// <summary>
        /// The size of the board (rectangle, so width = height).
        /// </summary>
        public static readonly int BoardWidthHeight = FieldWidthHeight * 8;

        /// <summary>
        /// The size of the promotion window in pixels.
        /// </summary>
        public static readonly Rectangle PromotionWindowSize = new Rectangle(0, 0, FieldWidthHeight * 4, FieldWidthHeight);

        /// <summary>
        /// The position of the promotion window in pixels.
        /// </summary>
        public static readonly Vector2 PromotionWindowPosition = (WindowSize - new Vector2(PromotionWindowSize.Width, PromotionWindowSize.Height)) / 2;
    }
}
