using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.MoveGenerators
{
    public class GeneratorParameters
    {
        public BitBoard BitBoard { get; set; }

        public Color FriendlyColor { get; set; }
        public Color EnemyColor { get; set; }
        public GeneratorMode Mode { get; set; }

        public ulong OccupancySummary { get; set; }
        public ulong FriendlyOccupancy { get; set; }
        public ulong EnemyOccupancy { get; set; }
    }
}
