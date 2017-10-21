using Proxima.Core.Boards.PatternGenerators;
using Proxima.Core.Commons;

namespace Proxima.Core
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
