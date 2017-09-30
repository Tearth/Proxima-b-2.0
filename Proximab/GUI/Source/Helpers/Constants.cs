using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.Helpers
{
    internal static class Constants
    {
        public static int FieldWidthHeight { get; } = 64;
        public static Rectangle FieldSize { get; } = new Rectangle(0, 0, FieldWidthHeight, FieldWidthHeight);
        public static Vector2 BoardPosition { get; } = new Vector2(0, 0);
    }
}
