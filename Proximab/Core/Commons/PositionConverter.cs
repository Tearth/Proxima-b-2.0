namespace Core.Commons
{
    public class PositionConverter
    {
        public PositionConverter()
        {

        }

        public Position Convert(string textNotation)
        {
            var fixedTextNotation = textNotation.Trim().ToLower();

            var x = 8 - ('h' - fixedTextNotation[0]);
            var y = 8 - ('8' - fixedTextNotation[1]);

            if (x < 1 || y < 1 || x > 8 || y > 8)
                return null;

            return new Position(x, y);
        }
    }
}
