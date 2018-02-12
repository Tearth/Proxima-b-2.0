using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.Transposition
{
    /// <summary>
    /// Represents a container of transposition node data.
    /// </summary>
    public class TranspositionNode
    {
        /// <summary>
        /// Gets or sets the node score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Get or sets the score type.
        /// </summary>
        public ScoreType Type { get; set; }

        /// <summary>
        /// Get or sets the node depth.
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Get or sets the best move for the current node.
        /// </summary>
        public Move BestMove { get; set; }
    }
}
