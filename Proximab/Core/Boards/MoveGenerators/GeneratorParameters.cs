using Core.Commons.Colors;
using Core.Commons.Moves;
using System.Collections.Generic;

namespace Core.Boards.MoveGenerators
{
    public class GeneratorParameters
    {
        public Color Color { get; set; }
        public GeneratorMode Mode { get; set; }

        public ulong Occupancy { get; set; }
        public ulong FriendlyOccupancy { get; set; }
        public ulong EnemyOccupancy { get; set; }

        public ulong[,] Pieces { get; set; }
        public ulong[,] Attacks { get; set; }
        public ulong[] EnPassant { get; set; }

        public LinkedList<Move> Moves { get; set; }
    }
}
