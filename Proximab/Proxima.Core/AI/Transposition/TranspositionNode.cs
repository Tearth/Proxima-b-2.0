using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.Transposition
{
    /// <summary>
    /// Represents a container of transposition node data.
    /// </summary>
    public class TranspositionNode
    {
        public int Score { get; set; }
        public ScoreType Type { get; set; }
        public int Depth { get; set; }

        public Move BestMove { get; set; }
    }
}
