namespace Core.Boards.MoveGenerators
{
    public static class PredefinedMoves
    {
        public static ulong[] Knight { get; private set; }
        public static ulong[] King { get; private set; }
        public static byte[,] Rook { get; private set; }

        static bool Ready = false;

        public static void GenerateMoves()
        {
            if (Ready)
                return;

            var knightMovesGenerator = new KnightMovesGenerator();
            var kingMovesGenerator = new KingMovesGenerator();
            var rookMovesGenerator = new RookMovesGenerator();

            Knight = knightMovesGenerator.Generate();
            King = kingMovesGenerator.Generate();
            Rook = rookMovesGenerator.Generate();

            Ready = true;
        }
    }
}
