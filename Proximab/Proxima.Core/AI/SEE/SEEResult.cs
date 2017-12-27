using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.AI.SEE
{
    /// <summary>
    /// Represents a container for the SEE result.
    /// </summary>
    public class SeeResult
    {
        /// <summary>
        /// Gets or sets the initial attacker type.
        /// </summary>
        public PieceType InitialAttackerType { get; set; }

        /// <summary>
        /// Gets or sets the attacked piece type.
        /// </summary>
        public PieceType AttackedPieceType { get; set; }

        /// <summary>
        /// Gets or sets the source position of initial attacker.
        /// </summary>
        public Position InitialAttackerFrom { get; set; }

        /// <summary>
        /// Gets or sets the destination position of initial attacker.
        /// </summary>
        public Position InitialAttackerTo { get; set; }

        /// <summary>
        /// Gets or sets the score of SEE.
        /// </summary>
        public int Score { get; set; }
    }
}
