using System;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI
{
    public class AIResult
    {
        public int Score { get; set; }
        public Move BestMove { get; set; }
        public AIStats Stats { get; set; }

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

        public AIResult()
        {
            Stats = new AIStats();
        }
    }
}
