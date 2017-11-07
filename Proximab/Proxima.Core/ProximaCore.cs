using Proxima.Core.MoveGenerators.MagicBitboards;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core
{
    public static class ProximaCore
    {
        public static void Init()
        {
            PatternsContainer.GeneratePatterns();
            MagicBitboardsContainer.GenerateAttacks();
        }
    }
}
