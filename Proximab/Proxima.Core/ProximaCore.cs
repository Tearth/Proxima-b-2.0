using Proxima.Core.Boards.Hashing;
using Proxima.Core.MoveGenerators.MagicBitboards;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core
{
    public static class ProximaCore
    {
        public static void Init()
        {
            PatternsContainer.GeneratePatterns();
            MagicContainer.LoadKeys();
            MagicContainer.GenerateAttacks();
            ZobristContainer.Init();
        }
    }
}
