using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.MoveGenerators
{
    /// <summary>
    /// Represents an container for the generator parameters.
    /// </summary>
    public class GeneratorParameters
    {
        /// <summary>
        /// Gets or sets the bitboard which will be updated.
        /// </summary>
        public Bitboard Bitboard { get; set; }

        /// <summary>
        /// Gets or sets the friendly color.
        /// </summary>
        public Color FriendlyColor { get; set; }

        /// <summary>
        /// Gets or sets the enemy color (<see cref="FriendlyColor"/> inversion).
        /// </summary>
        public Color EnemyColor { get; set; }

        /// <summary>
        /// Gets or sets the generator mode.
        /// </summary>
        public GeneratorMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the occupancy summary.
        /// </summary>
        public ulong OccupancySummary { get; set; }

        /// <summary>
        /// Gets or sets the friendly occupancy.
        /// </summary>
        public ulong FriendlyOccupancy { get; set; }

        /// <summary>
        /// Gets or sets the enemy occupancy.
        /// </summary>
        public ulong EnemyOccupancy { get; set; }

        public bool QuiescenceSearch { get; set; }
    }
}
