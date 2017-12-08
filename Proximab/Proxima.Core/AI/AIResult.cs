using System;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI
{
    /// <summary>
    /// Represents a container for AI result.
    /// </summary>
    public class AIResult
    {
        /// <summary>
        /// Gets or sets the evaluation score of best move.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the best move for the specified bitboard..
        /// </summary>
        public Move BestMove { get; set; }

        /// <summary>
        /// Gets or sets the AI stats.
        /// </summary>
        public AIStats Stats { get; set; }

        /// <summary>
        /// Gets or sets the time of AI calculating.
        /// </summary>
        public long Ticks { get; set; }

        /// <summary>
        /// Gets the total time required for whole test.
        /// </summary>
        public float Time
        {
            get { return (float)new TimeSpan(Ticks).TotalSeconds; }
        }

        /// <summary>
        /// Gets the number of nodes per second (TotalNodes / Time).
        /// </summary>
        public int NodesPerSecond
        {
            get { return Time != 0 ? (int)(Stats.TotalNodes / Time) : 0; }
        }

        /// <summary>
        /// Gets the number of nanoseconds per node ((Ticks / TotalNodes) * 100).
        /// </summary>
        public int TimePerNode
        {
            get { return Stats.TotalNodes != 0 ? (int)(Ticks / Stats.TotalNodes) * 100 : 0; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AIResult"/> class.
        /// </summary>
        public AIResult()
        {
            Stats = new AIStats();
        }
    }
}
