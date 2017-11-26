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
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}
