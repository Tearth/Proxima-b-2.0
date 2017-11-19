using System;

namespace Proxima.Helpers.Tests
{
    /// <summary>
    /// Container for moves test data.
    /// </summary>
    public class MovesTestData
    {
        /// <summary>
        /// Number of total nodes (includes end nodes)
        /// </summary>
        public int TotalNodes { get; set; }

        /// <summary>
        /// Number of end nodes
        /// </summary>
        public int EndNodes { get; set; }

        /// <summary>
        /// Number of ticks required for whole test.
        /// </summary>
        public long Ticks { get; set; }

        /// <summary>
        /// Flag indicating whether all boards were correctly generated.
        /// </summary>
        public bool Integrity { get; set; }

        /// <summary>
        /// Total time required for whole test.
        /// </summary>
        public float Time
        {
            get { return (float)new TimeSpan(Ticks).TotalSeconds; }
        }

        /// <summary>
        /// Number of nodes per second (TotalNodes / Time).
        /// </summary>
        public int NodesPerSecond
        {
            get { return Time != 0 ? (int)(TotalNodes / Time) : 0; }
        }

        /// <summary>
        /// Number of nanoseconds per node ((Ticks / TotalNodes) * 100).
        /// </summary>
        public int TimePerNode
        {
            get { return TotalNodes != 0 ? (int)(Ticks / TotalNodes) * 100 : 0; }
        }

        public MovesTestData()
        {
            Integrity = true;
        }
    }
}
