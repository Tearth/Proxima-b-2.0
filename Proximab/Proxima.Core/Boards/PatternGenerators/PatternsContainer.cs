namespace Proxima.Core.Boards.PatternGenerators
{
    public static class PatternsContainer
    {
        public static ulong[] KnightPattern { get; private set; }
        public static ulong[] KingPattern { get; private set; }
        public static byte[] SlidePattern { get; private set; }
        
        public static void GeneratePatterns()
        {
            var knightPatternGenerator = new KnightPatternGenerator();
            var kingPatternGenerator = new KingPatternGenerator();
            var slidePatternGenerator = new SlidePatternGenerator();

            KnightPattern = knightPatternGenerator.Generate();
            KingPattern = kingPatternGenerator.Generate();
            SlidePattern = slidePatternGenerator.Generate();
        }
    }
}
