using System;

namespace Proxima.Core.Tests
{
    /// <summary>
    /// Container for moves test data.
    /// </summary>
    public class MovesTestData
    {
        /// <summary>
        /// Gets or sets the number of total nodes (includes end nodes)
        /// </summary>
        public int TotalNodes { get; set; }

        /// <summary>
        /// Gets or sets the number of end nodes
        /// </summary>
        public int EndNodes { get; set; }

        /// <summary>
        /// Gets or sets the number of ticks required for whole test.
        /// </summary>
        public long Ticks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all boards were correctly generated.
        /// </summary>
        public bool Integrity { get; set; }

        /// <summary>
        /// Gets the total time required for whole test.
        /// </summary>
        public float Time => (float)new TimeSpan(Ticks).TotalSeconds;

        /// <summary>
        /// Gets the number of nodes per second (TotalNodes / Time).
        /// </summary>
        public int NodesPerSecond => (int)Time != 0 ? (int)(TotalNodes / Time) : 0;

        /// <summary>
        /// Gets the number of nanoseconds per node ((Ticks / TotalNodes) * 100).
        /// </summary>
        public int TimePerNode => TotalNodes != 0 ? (int)(Ticks / TotalNodes) * 100 : 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovesTestData"/> class.
        /// </summary>
        public MovesTestData()
        {
            Integrity = true;
        }
    }
}
