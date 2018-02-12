using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.Search
{
    /// <summary>
    /// Represents a regular sorted move (move with assigned score which will be used to sort).
    /// </summary>
    public class RegularSortedMove
    {
        /// <summary>
        /// Gets or sets the move.
        /// </summary>
        public Move Move { get; set; }

        /// <summary>
        /// Gets or sets the score which will be used to sort.
        /// </summary>
        public int Score { get; set; }
    }
}
