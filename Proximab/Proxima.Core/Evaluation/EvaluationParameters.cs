namespace Proxima.Core.Evaluation
{
    public class EvaluationParameters
    {
        public ulong[] Pieces { get; set; }
        public ulong[] Occupancy { get; set; }
        public ulong[] EnPassant { get; set; }
        public bool[] Castling { get; set; }

        public ulong[] Attacks { get; set; }
        public ulong[] AttacksSummary { get; set; }
    }
}
