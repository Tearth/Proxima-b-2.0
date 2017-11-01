using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using System.Collections.Generic;

namespace Proxima.Core.MoveGenerators
{
    public class GeneratorParameters
    {
        public Color FriendlyColor { get; set; }
        public Color EnemyColor { get; set; }
        public GeneratorMode Mode { get; set; }

        public ulong[] Pieces { get; set; }
        public bool[] Castling { get; set; }
        public ulong[] EnPassant { get; set; }

        public ulong[] Attacks { get; set; }
        public ulong[] AttacksSummary { get; set; }

        public ulong Occupancy { get; set; }
        public ulong FriendlyOccupancy { get; set; }
        public ulong EnemyOccupancy { get; set; }

        public LinkedList<Move> Moves { get; set; }
    }
}
