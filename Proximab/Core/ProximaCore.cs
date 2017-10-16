using Core.Boards.MoveGenerators;
using Core.Commons;

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
