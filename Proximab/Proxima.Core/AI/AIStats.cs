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

        /// <summary>
        /// Gets or sets the beta cut-offs.
        /// </summary>
        public int AlphaBetaCutoffs { get; set; }

        /// <summary>
        /// Gets or sets the transposition table hits.
        /// </summary>
        public int TranspositionTableHits { get; set; }

        /// <summary>
        /// Gets or sets the quiescence total nodes count.
        /// </summary>
        public int QuiescenceTotalNodes { get; set; }

        /// <summary>
        /// Gets or sets the quiescence end nodes count.
        /// </summary>
        public int QuiescenceEndNodes { get; set; }
    }
}
