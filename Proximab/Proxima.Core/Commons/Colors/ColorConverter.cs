using Proxima.Core.Commons.Exceptions;

namespace Proxima.Core.Commons.Colors
{
    public static class ColorConverter
    {
        public const char WhiteColorSymbol = 'W';
        public const char BlackColorSymbol = 'B';

        public static char GetSymbol(Color color)
        {
            switch(color)
            {
                case Color.White: return WhiteColorSymbol;
                case Color.Black: return BlackColorSymbol;
            }

            throw new ColorSymbolNotFoundException();
        }

        public static Color GetColor(char color)
        {
            switch(color)
            {
                case WhiteColorSymbol: return Color.White;
                case BlackColorSymbol: return Color.Black;
            }

            throw new ColorSymbolNotFoundException();
        }
    }
}
