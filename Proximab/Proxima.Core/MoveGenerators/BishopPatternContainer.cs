namespace Proxima.Core.MoveGenerators
{
    public class BishopPatternContainer
    {
        public ulong A1H8Diagonal { get; private set; }
        public ulong A8H1Diagonal { get; private set; }

        public BishopPatternContainer()
        {

        }

        public BishopPatternContainer(ulong a1h8Diagonal, ulong a8h1Diagonal)
        {
            A1H8Diagonal = a1h8Diagonal;
            A8H1Diagonal = a8h1Diagonal;
        }
    }
}
