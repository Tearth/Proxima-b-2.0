using System.Runtime.CompilerServices;

namespace Proxima.Core.Commons.Colors
{
    public static class ColorOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Invert(Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}
