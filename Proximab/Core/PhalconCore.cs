using Core.MoveGenerators;

namespace Core
{
    public static class PhalconCore
    {
        static bool Ready = false;

        public static void Init()
        {
            if (Ready)
                return;

            PredefinedMoves.GenerateMoves();

            Ready = true;
        }
    }
}
