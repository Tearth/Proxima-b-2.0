namespace Core.Boards.MoveGenerators
{
    public static class PredefinedMoves
    {
        public static ulong[] Knight { get; private set; }
        public static ulong[] King { get; private set; }

        static bool Ready = false;

        public static void GenerateMoves()
        {
            if (Ready)
                return;

            var knightMovesGenerator = new KnightMovesGenerator();
            var kingMovesGenerator = new KingMovesGenerator();

            Knight = knightMovesGenerator.Generate();
            King = kingMovesGenerator.Generate();

            Ready = true;
        }
    }
}
