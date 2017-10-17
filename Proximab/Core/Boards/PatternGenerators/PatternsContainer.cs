namespace Core.Boards.PatternGenerators
{
    public static class PatternsContainer
    {
        public static ulong[] KnightPattern { get; private set; }
        public static ulong[] KingPattern { get; private set; }
        public static byte[,] SlidePattern { get; private set; }

        static bool Ready = false;

        public static void GeneratePatterns()
        {
            if (Ready)
                return;

            var knightPatternGenerator = new KnighPatternGenerator();
            var kingPatternGenerator = new KingPatternGenerator();
            var slidePatternGenerator = new SlidePatternGenerator();

            KnightPattern = knightPatternGenerator.Generate();
            KingPattern = kingPatternGenerator.Generate();
            SlidePattern = slidePatternGenerator.Generate();

            Ready = true;
        }
    }
}
