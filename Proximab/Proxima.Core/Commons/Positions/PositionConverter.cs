using Proxima.Core.Commons.Exceptions;

namespace Proxima.Core.Commons.Positions
{
    /// <summary>
    /// Represents a set of methods to convert <see cref="Position"/> class instance to/from another form.
    /// </summary>
    public static class PositionConverter
    {
        /// <summary>
        /// Converts a position into the text notation.
        /// </summary>
        /// <param name="position">The field position.</param>
        /// <returns>The text notation.</returns>
        public static string ToString(Position position)
        {
            var file = ((char)('a' + position.X - 1)).ToString();
            var rank = position.Y.ToString();

            return file + rank;
        }

        /// <summary>
        /// Converts a text notation into the <see cref="Position"/> object.
        /// </summary>
        /// <param name="textNotation">Text text notation.</param>
        /// <exception cref="PositionOutOfRangeException">Thrown when a text notation cannot be converted properly to <see cref="Position"/> object.</exception>
        /// <returns>The field position.</returns>
        public static Position ToPosition(string textNotation)
        {
            var fixedTextNotation = textNotation.Trim().ToLower();

            var x = 8 - ('h' - fixedTextNotation[0]);
            var y = 8 - ('8' - fixedTextNotation[1]);

            var position = new Position(x, y);
            if (!position.IsValid())
            {
                throw new PositionOutOfRangeException();
            }

            return position;
        }
    }
}
