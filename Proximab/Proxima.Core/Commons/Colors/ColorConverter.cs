using Proxima.Core.Commons.Exceptions;

namespace Proxima.Core.Commons.Colors
{
    /// <summary>
    /// Represents a set of methods to converting between <see cref="Color"/> and text notations.
    /// </summary>
    public static class ColorConverter
    {
        public const char WhiteColorSymbol = 'W';
        public const char BlackColorSymbol = 'B';

        /// <summary>
        /// Converts a color symbol to the associated symbol.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The color symbol.</returns>
        public static char GetSymbol(Color color)
        {
            switch (color)
            {
                case Color.White: return WhiteColorSymbol;
                case Color.Black: return BlackColorSymbol;
            }

            throw new ColorSymbolNotFoundException();
        }

        /// <summary>
        /// Converts a color symbol to the <see cref="Color"/> object.
        /// </summary>
        /// <param name="color">The color symbol.</param>
        /// <exception cref="ColorSymbolNotFoundException">Thrown when the specified color is not recognized.</exception>
        /// <returns>The <see cref="Color"/> object.</returns>
        public static Color GetColor(char color)
        {
            switch (color)
            {
                case WhiteColorSymbol: return Color.White;
                case BlackColorSymbol: return Color.Black;
            }

            throw new ColorSymbolNotFoundException();
        }
    }
}
