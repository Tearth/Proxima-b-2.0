using Proxima.Core.Boards.PatternGenerators;

namespace Proxima.Core
{
    public static class ProximaCore
    {
        public static void Init()
        {
            PatternsContainer.GeneratePatterns();
        }
    }
}
