using GUI.Source.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.BoardSubsystem.Axes
{
    internal class AxesManager
    {
        public Color Color { get; set; }
        public Vector2 HorizontalOffset { get; set; }
        public Vector2 VerticalOffset { get; set; }

        SpriteFont _axeFont;

        public AxesManager()
        {
            Color = Color.White;
            HorizontalOffset = new Vector2(0, -9);
            VerticalOffset = new Vector2(-9, -1);
        }

        public void LoadContent(ContentManager content)
        {
            _axeFont = content.Load<SpriteFont>("Fonts\\AxisFont");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHorizontalAxes(spriteBatch);
            DrawVerticalAxes(spriteBatch);
        }

        void DrawVerticalAxes(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 8; i++)
            {
                var text = (8 - i).ToString();
                var textCenterOffset = GetCenterOffset(text);

                var position = new Vector2(0, (i * Constants.FieldWidthHeight));

                position.Y += Constants.FieldWidthHeight / 2;
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

        void DrawHorizontalAxes(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 8; i++)
            {
                var text = ((char)('A' + i)).ToString();
                var textCenterOffset = GetCenterOffset(text);

                var position = new Vector2((i * Constants.FieldWidthHeight), 0);

                position.X += Constants.FieldWidthHeight / 2;
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

        Vector2 GetCenterOffset(string text)
        {
            var textSize = _axeFont.MeasureString(text) / 2;
            textSize.X = (float)Math.Round(textSize.X);
            textSize.Y = (float)Math.Round(textSize.Y);

            return textSize;
        }
    }
}
