using Core.Boards.PatternGenerators;
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
            
            PatternsContainer.GeneratePatterns();

            Ready = true;
        }
    }
}
