namespace Proxima.Core.Commons.Colors
{
    public static class ColorConverter
    {
        public const char WhiteColorSymbol = 'W';
        public const char BlackColorSymbol = 'B';

        public static char GetSymbol(Color color)
        {
            return color == Color.White ? WhiteColorSymbol : BlackColorSymbol;
        }

        public static Color GetColor(char color)
        {
            switch(color)
            {
                case WhiteColorSymbol: return Color.White;
                case BlackColorSymbol: return Color.Black;
            }

            return Color.White;
        }
    }
}
