using System;
using System.Collections.Generic;
using Proxima.Core.Commons.Colors;
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

        public PVNodesList PVNodes { get; set; }

        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the maximal depth of any calculated node.
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Gets or sets the AI stats.
        /// </summary>
        public AIStats Stats { get; set; }

        /// <summary>
        /// Gets or sets the time of AI calculating.
        /// </summary>
        public long Ticks { get; set; }

        /// <summary>
        /// Gets or sets the preferred time (AI will try to not exceed this when calculating best move).
        /// </summary>
        public float PreferredTime { get; set; }

        /// <summary>
        /// Gets the total time required for whole test.
        /// </summary>
        public float Time => (float)new TimeSpan(Ticks).TotalSeconds;

        /// <summary>
        /// Gets the number of nodes per second (TotalNodes / Time).
        /// </summary>
        public int NodesPerSecond => Math.Abs(Time) > float.Epsilon ? (int)(Stats.TotalNodes / Time) : 0;

        /// <summary>
        /// Gets the number of nanoseconds per node ((Ticks / TotalNodes) * 100).
        /// </summary>
        public int TimePerNode => Stats.TotalNodes != 0 ? (int)(Ticks / Stats.TotalNodes) * 100 : 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIResult"/> class.
        /// </summary>
        public AIResult()
        {
            Stats = new AIStats();
            PVNodes = new PVNodesList();
        }
    }
}
