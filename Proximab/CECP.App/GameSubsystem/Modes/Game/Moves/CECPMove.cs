using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace CECP.App.GameSubsystem.Modes.Game.Moves
{
    /// <summary>
    /// Represents a CECP move object.
    /// </summary>
    public class CECPMove
    {
        /// <summary>
        /// Gets the source piece position.
        /// </summary>
        public Position From { get; }

        /// <summary>
        /// Gets the destination piece position.
        /// </summary>
        public Position To { get; }

        /// <summary>
        /// Gets the promotion piece type (null in move is not promoting anything).
        /// </summary>
        public PieceType? PromotionPiece { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CECPMove"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        public CECPMove(Position from, Position to) : this(from, to, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CECPMove"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="promotionPiece">The promotion piece type (null if move is not promoting anything).</param>
        public CECPMove(Position from, Position to, PieceType? promotionPiece)
        {
            From = from;
            To = to;
            PromotionPiece = promotionPiece;
        }
    }
}
