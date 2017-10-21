namespace Proxima.Core.Boards.MoveGenerators
{
    public class RookPatternContainer
    {
        public ulong Horizontal { get; set; }
        public ulong Vertical { get; set; }

        public RookPatternContainer() : this(0, 0)
        {

        }

        public RookPatternContainer(ulong horizontal, ulong vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}
