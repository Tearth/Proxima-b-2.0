using System;
using GUI.App.Source.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GUI.App.Source.BoardSubsystem.Axes
{
    /// <summary>
    /// Represents a set of methods for displaying board axes.
    /// </summary>
    internal class AxesManager
    {
        /// <summary>
        /// Gets or sets the color of axe chars.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the offset of horizontal axes.
        /// </summary>
        public Vector2 HorizontalOffset { get; set; }

        /// <summary>
        /// Gets or sets the offset of vertial axes.
        /// </summary>
        public Vector2 VerticalOffset { get; set; }

        private SpriteFont _axeFont;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxesManager"/> class.
        /// </summary>
        public AxesManager()
        {
            Color = Color.White;
            HorizontalOffset = new Vector2(0, -9);
            VerticalOffset = new Vector2(-9, -1);
        }

        /// <summary>
        /// Loads resources. Must be called before first use.
        /// </summary>
        /// <param name="content">Monogame content manager</param>
        public void LoadContent(ContentManager content)
        {
            _axeFont = content.Load<SpriteFont>("Fonts\\AxisFont");
        }

        /// <summary>
        /// Draws board axes
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHorizontalAxes(spriteBatch);
            DrawVerticalAxes(spriteBatch);
        }

        /// <summary>
        /// Draws vertical axes
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch</param>
        private void DrawVerticalAxes(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 8; i++)
            {
                var text = (8 - i).ToString();
                var textCenterOffset = GetCenterOffset(text);

                var position = new Vector2(0, i * Constants.FieldWidthHeight);

                position.Y += Constants.FieldWidthHeight >> 1;
                position += Constants.BoardPosition;
                position -= textCenterOffset;

                var leftAxePosition = position + VerticalOffset;

                var rightAxePosition = position;
                rightAxePosition += new Vector2(Constants.BoardWidthHeight, 0);
                rightAxePosition += new Vector2(-VerticalOffset.X, VerticalOffset.Y);

                spriteBatch.DrawString(_axeFont, text, leftAxePosition, Color);
                spriteBatch.DrawString(_axeFont, text, rightAxePosition, Color);
            }
        }

        /// <summary>
        /// Draws horizontal axes.
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch</param>
        private void DrawHorizontalAxes(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 8; i++)
            {
                var text = ((char)('A' + i)).ToString();
                var textCenterOffset = GetCenterOffset(text);

                var position = new Vector2(i * Constants.FieldWidthHeight, 0);

                position.X += Constants.FieldWidthHeight >> 1;
                position += Constants.BoardPosition;
                position -= textCenterOffset;

                var topAxePosition = position + HorizontalOffset;

                var downAxePosition = position;
                downAxePosition += new Vector2(0, Constants.BoardWidthHeight);
                downAxePosition += new Vector2(HorizontalOffset.X, -HorizontalOffset.Y);

                spriteBatch.DrawString(_axeFont, text, topAxePosition, Color);
                spriteBatch.DrawString(_axeFont, text, downAxePosition, Color);
            }
        }

        /// <summary>
        /// Calculates center of the specified text (based on the loaded font).
        /// </summary>
        /// <param name="text">Text to calculate.</param>
        /// <returns>Center of passed text.</returns>
        private Vector2 GetCenterOffset(string text)
        {
            var textSize = _axeFont.MeasureString(text) / 2;
            textSize.X = (float)Math.Round(textSize.X);
            textSize.Y = (float)Math.Round(textSize.Y);

            return textSize;
        }
    }
}
