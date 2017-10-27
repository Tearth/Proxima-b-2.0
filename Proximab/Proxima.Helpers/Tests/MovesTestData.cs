using System;

namespace Proxima.Helpers.Tests
{
    public class MovesTestData
    {
        public int TotalNodes { get; set; }
        public int EndNodes { get; set; }
        public long Ticks { get; set; }

        public float Time
        {
            get { return (float)new TimeSpan(Ticks).TotalSeconds; }
        }

        public int NodesPerSecond
        {
            get { return Time != 0 ? (int)(TotalNodes / Time) : 0; }
        }

        public int TimePerNode
        {
            get { return TotalNodes != 0 ? (int)(Ticks / TotalNodes) * 100 : 0; }
        }
    }
}
