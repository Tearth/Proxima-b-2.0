namespace Proxima.Core.MoveGenerators.PatternGenerators
{
    public static class PatternsContainer
    {
        public static ulong[] KnightPattern { get; private set; }
        public static ulong[] KingPattern { get; private set; }
        public static ulong[] RookPattern { get; private set; }
        public static byte[] SlidePattern { get; private set; }
        public static ulong[] BishopPattern { get; private set; }

        public static void GeneratePatterns()
        {
            KnightPattern = new KnightPatternGenerator().Generate();
            KingPattern = new KingPatternGenerator().Generate();
            RookPattern = new RookPatternGenerator().Generate();
            BishopPattern = new BishopPatternGenerator().Generate();

            SlidePattern = new byte[64];
        }
    }
}
