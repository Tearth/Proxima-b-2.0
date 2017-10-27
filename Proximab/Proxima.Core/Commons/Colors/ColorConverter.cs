using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Commons.Colors
{
    public static class ColorConverter
    {
        public static string GetSymbol(Color color)
        {
            return color == Color.White ? "W" : "B";
        }

        public static Color GetColor(string color)
        {
            switch(color)
            {
                case "W": return Color.White;
                case "B": return Color.Black;
            }

            return Color.White;
        }
    }
}
