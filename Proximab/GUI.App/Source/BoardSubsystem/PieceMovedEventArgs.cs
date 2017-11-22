using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Positions;

namespace GUI.App.Source.BoardSubsystem
{
    /// <summary>
    /// Represents information about PieceMoved event.
    /// </summary>
    internal class PieceMovedEventArgs
    {
        /// <summary>
        /// Gets the moved piece.
        /// </summary>
        public FriendlyPiece Piece { get; private set; }

        /// <summary>
        /// Gets the source position of the piece.
        /// </summary>
        public Position From { get; private set; }

        /// <summary>
        /// Gets the destination position of the piece.
        /// </summary>
        public Position To { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceMovedEventArgs"/> class.
        /// </summary>
        /// <param name="piece">The moved piece</param>
        /// <param name="from">The source position of the piece.</param>
        /// <param name="to">The destination position of the piece.</param>
        public PieceMovedEventArgs(FriendlyPiece piece, Position from, Position to)
        {
            Piece = piece;
            From = from;
            To = to;
        }
    }
}
