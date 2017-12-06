namespace Proxima.Core.MoveGenerators.PatternGenerators
{
    /// <summary>
    /// Represents a container for all piece patterns.
    /// </summary>
    public static class PatternsContainer
    {
        /// <summary>
        /// Gets the knight pattern.
        /// </summary>
        public static ulong[] KnightPattern { get; private set; }

        /// <summary>
        /// Gets the king pattern.
        /// </summary>
        public static ulong[] KingPattern { get; private set; }

        /// <summary>
        /// Gets the rook pattern.
        /// </summary>
        public static ulong[] RookPattern { get; private set; }

        /// <summary>
        /// Gets the bishop pattern.
        /// </summary>
        public static ulong[] BishopPattern { get; private set; }

        /// <summary>
        /// Inits all piece patterns.
        /// </summary>
        public static void Init()
        {
            KnightPattern = new KnightPatternGenerator().Generate();
            KingPattern = new KingPatternGenerator().Generate();
            RookPattern = new RookPatternGenerator().Generate();
            BishopPattern = new BishopPatternGenerator().Generate();
        }
    }
}
