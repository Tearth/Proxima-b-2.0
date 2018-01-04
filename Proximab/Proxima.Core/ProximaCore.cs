using Proxima.Core.MoveGenerators.MagicBitboards;
using Proxima.Core.MoveGenerators.PatternGenerators;
using Proxima.Core.OpeningBook;

namespace Proxima.Core
{
    /// <summary>
    /// Represents a set of methods to initialize chess engine.
    /// </summary>
    public static class ProximaCore
    {
        /// <summary>
        /// Initializes chess engine (loads patterns, magic keys, sets converters, ...).
        /// Must be called first, otherwise chess engine will crash :(
        /// </summary>
        public static void Init()
        {
            PatternsContainer.Init();
            MagicContainer.Init();
            OpeningBookContainer.Init();
        }
    }
}
