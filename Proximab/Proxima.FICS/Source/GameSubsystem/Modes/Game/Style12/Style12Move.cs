using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.FICS.Source.GameSubsystem.Modes.Game.Style12
{
    /// <summary>
    /// Represents a Style12 move object.
    /// </summary>
    public class Style12Move
    {
        /// <summary>
        /// Gets the piece type participating in the move.
        /// </summary>
        public PieceType PieceType { get; private set; }

        /// <summary>
        /// Gets the source piece position.
        /// </summary>
        public Position From { get; private set; }

        /// <summary>
        /// Gets the destination piece position.
        /// </summary>
        public Position To { get; private set; }

        /// <summary>
        /// Gets the piece type after promotion. Null if the piece is not promoted.
        /// </summary>
        public PieceType? PromotionPieceType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Style12Move"/> class.
        /// </summary>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        public Style12Move(PieceType pieceType, Position from, Position to)
        {
            PieceType = pieceType;
            From = from;
            To = to;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Style12Move"/> class.
        /// </summary>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="promotionPieceType">The piece type after promotion.</param>
        public Style12Move(PieceType pieceType, Position from, Position to, PieceType promotionPieceType)
            : this(pieceType, from, to)
        {
            PromotionPieceType = promotionPieceType;
        }
    }
}
