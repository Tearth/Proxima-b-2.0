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

        public int BranchingFactor
        {
            get
            {
                return (TotalNodes - 1) / (TotalNodes - EndNodes);
            }
        }
    }
}
