namespace Core.Boards.MoveGenerators
{
    public static class PredefinedMoves
    {
        public static ulong[] KnightMoves { get; private set; }
        public static ulong[] KingMoves { get; private set; }
        public static byte[,] SlideMoves { get; private set; }

        static bool Ready = false;

        public static void GenerateMoves()
        {
            if (Ready)
                return;

            var knightMovesGenerator = new KnightMovesGenerator();
            var kingMovesGenerator = new KingMovesGenerator();
            var slideMovesGenerator = new SlideMovesGenerator();

            KnightMoves = knightMovesGenerator.Generate();
            KingMoves = kingMovesGenerator.Generate();
            SlideMoves = slideMovesGenerator.Generate();

            Ready = true;
        }
    }
}
