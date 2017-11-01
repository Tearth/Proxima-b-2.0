namespace Proxima.Core.MoveGenerators
{
    public class RookPatternContainer
    {
        public ulong Horizontal { get; private set; }
        public ulong Vertical { get; private set; }

        public RookPatternContainer()
        {

        }

        public RookPatternContainer(ulong horizontal, ulong vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}
