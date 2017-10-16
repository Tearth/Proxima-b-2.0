using Core.Boards.MoveGenerators;

namespace Core
{
    public static class ProximaCore
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
