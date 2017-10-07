namespace Core.MoveGenerators
{
    public static class PredefinedMoves
    {
        static bool Ready = false;

        static ulong[] Knight;

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
