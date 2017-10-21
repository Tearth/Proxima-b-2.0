using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using System.Collections.Generic;

namespace Proxima.Core.Boards.MoveGenerators
{
    public class GeneratorParameters
    {
        public Color Color { get; set; }
        public GeneratorMode Mode { get; set; }
        public CastlingData CastlingData { get; set; }

        public ulong Occupancy { get; set; }
        public ulong FriendlyOccupancy { get; set; }
        public ulong EnemyOccupancy { get; set; }

        public ulong[,] Pieces { get; set; }
        public ulong[] EnPassant { get; set; }

        public ulong[,,] Attacks;
        public ulong[] AttacksSummary;

        public LinkedList<Move> Moves { get; set; }
    }
}
