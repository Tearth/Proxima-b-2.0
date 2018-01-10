namespace Proxima.Core.AI
{
    /// <summary>
    /// Represents a container of AI stats.
    /// </summary>
    public class AIStats
    {
        /// <summary>
        /// Gets or sets the total nodes.
        /// </summary>
        public int TotalNodes { get; set; }

        /// <summary>
        /// Gets or sets the end nodes.
        /// </summary>
        public int EndNodes { get; set; }

        /// <summary>
        /// Gets the branching factor (average number of children per node).
        /// </summary>
        public int BranchingFactor => TotalNodes > EndNodes ? (TotalNodes - 1) / (TotalNodes - EndNodes) : 1;

        public int AlphaBetaCutoffs { get; set; }

        public int TranspositionTableHits { get; set; }

        public int QuiescenceTotalNodes { get; set; }

        public int QuiescenceEndNodes { get; set; }
    }
}
