using Proxima.Core.Commons.Positions;
using System;

namespace Proxima.Core.Commons.Notation
{
    public static class NotationConverter
    {
        public static string ToString(Position position)
        {
            var file = ('a' + (position.X - 1)).ToString();
            var rank = position.Y.ToString();

            return file + rank; 
        }

        public static Position ToPosition(string stringPosition)
        {
            var fixedStringPosition = stringPosition.ToLower();

            var file = 'h' - fixedStringPosition[0] + 1;
            var rank = '8' - fixedStringPosition[1] + 1;

            return new Position(file, rank);
        }
    }
}
