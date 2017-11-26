using Proxima.Core.Commons.Exceptions;
using Proxima.Core.Helpers.Bidirectional;

namespace Proxima.Core.Commons.Colors
{
    /// <summary>
    /// Represents a set of methods to converting between <see cref="Color"/> and text notations.
    /// </summary>
    public static class ColorConverter
    {
        private static BidirectionalDictionary<Color, char> _colors;

        /// <summary>
        /// Inits internal dictionaries.
        /// </summary>
        public static void Init()
        {
            _colors = new BidirectionalDictionary<Color, char>();

            _colors.Add(Color.White, 'W');
            _colors.Add(Color.Black, 'B');
        }

        /// <summary>
        /// Converts a color symbol to the associated symbol.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The color symbol.</returns>
        public static char GetSymbol(Color color)
        {
            if (!_colors.Forward.ContainsKey(color))
            {
                throw new ColorSymbolNotFoundException();
            }

            return _colors.Forward[color];
        }

        /// <summary>
        /// Converts a color symbol to the <see cref="Color"/> object.
        /// </summary>
        /// <param name="colorSymbol">The color symbol.</param>
        /// <exception cref="ColorSymbolNotFoundException">Thrown when the specified color is not recognized.</exception>
        /// <returns>The <see cref="Color"/> object.</returns>
        public static Color GetColor(char colorSymbol)
        {
            if (!_colors.Reverse.ContainsKey(colorSymbol))
            {
                throw new ColorSymbolNotFoundException();
            }

            return _colors.Reverse[colorSymbol];
        }
    }
}
