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

        SpriteFont _axexFont;

        public AxesManager()
        {
            Color = Color.White;
            HorizontalOffset = new Vector2(0, 12);
            VerticalOffset = new Vector2(-9, 3);
        }

        public void LoadContent(ContentManager content)
        {
            _axexFont = content.Load<SpriteFont>("Fonts\\AxisFont");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHorizontalLine(spriteBatch);
            DrawVerticalLine(spriteBatch);
        }

        void DrawVerticalLine(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 8; i++)
            {
                var text = (8 - i).ToString();
                var textCenterOffset = GetCenterOffset(text);

                var position = new Vector2()
                {
                    X = 0,
                    Y = (i * Constants.FieldWidthHeight) + (Constants.FieldWidthHeight / 2)
                };

                position += Constants.BoardPosition;
                position -= textCenterOffset;
                position += VerticalOffset;

                spriteBatch.DrawString(_axexFont, text, position, Color);
            }
        }

        void DrawHorizontalLine(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 8; i++)
            {
                var text = ((char)('A' + i)).ToString();
                var textCenterOffset = GetCenterOffset(text);

                var position = new Vector2()
                {
                    X = (i * Constants.FieldWidthHeight) + (Constants.FieldWidthHeight / 2),
                    Y = Constants.WindowSize.Y
                };

                position += new Vector2(Constants.BoardPosition.X, -Constants.BoardPosition.Y);
                position -= textCenterOffset;
                position += HorizontalOffset;

                spriteBatch.DrawString(_axexFont, text, position, Color);
            }
        }

        Vector2 GetCenterOffset(string text)
        {
            return _axexFont.MeasureString(text) / 2;
        }
    }
}
