using Proxima.Core.Commons.Exceptions;

namespace Proxima.Core.Commons.Positions
{
    public static class PositionConverter
    {
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
