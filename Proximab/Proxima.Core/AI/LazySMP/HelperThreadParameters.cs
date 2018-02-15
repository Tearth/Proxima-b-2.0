using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.AI.LazySMP
{
    /// <summary>
    /// Represents a container for regular search helper parameters.
    /// </summary>
    public struct HelperTaskParameters
    {
        /// <summary>
        /// Gets or sets the initial color of search.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the bitboard.
        /// </summary>
        public Bitboard Bitboard { get; set; }

        /// <summary>
        /// Gets or sets the initial depth.
        /// </summary>
        public int InitialDepth { get; set; }

        /// <summary>
        /// Gets or sets the time after which search is immediately terminated.
        /// </summary>
        public long Deadline { get; set; }
    }
}
