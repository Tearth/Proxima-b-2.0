using System.Runtime.CompilerServices;

namespace Proxima.Core.Commons.Colors
{
    /// <summary>
    /// Represents a set of methods to manipulate <see cref="Color"/> objects.
    /// </summary>
    public static class ColorOperations
    {
        /// <summary>
        /// Inverts a color to the opposite one (White -> Black, Black -> White).
        /// </summary>
        /// <param name="color">The color to invert.</param>
        /// <returns>The inverted color.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Invert(Color color)
        {
            return (Color)((int)color ^ 1);
        }

        /// <summary>
        /// Converts a color to sign (White = 1, Black = -1).
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The color sign.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToSign(Color color)
        {
            return -(((int)color * 2) - 1);
        }
    }
}
