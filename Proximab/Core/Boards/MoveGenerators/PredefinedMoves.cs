namespace Core.Boards.MoveGenerators
{
    public static class PredefinedMoves
    {
        public static ulong[] Knight { get; private set; }

        static bool Ready = false;

        public static void GenerateMoves()
        {
            if (Ready)
                return;

            var knightMovesGenerator = new KnightMovesGenerator();

            Knight = knightMovesGenerator.Generate();

            Ready = true;
        }
    }
}
