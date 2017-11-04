using Proxima.Core.Commons;

namespace Proxima.Core.Evaluation
{
    public class EvaluationParameters
    {
        public GamePhase GamePhase { get; set; }

        public ulong[] Pieces { get; set; }
        public ulong[] Occupancy { get; set; }
        public ulong[] EnPassant { get; set; }
        public bool[] CastlingDone { get; set; }

        public ulong[] Attacks { get; set; }
        public ulong[] AttacksSummary { get; set; }
    }
}
