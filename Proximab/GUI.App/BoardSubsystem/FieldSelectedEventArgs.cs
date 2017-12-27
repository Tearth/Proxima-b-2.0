using System;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Positions;

namespace GUI.App.BoardSubsystem
{
    /// <summary>
    /// Represents information about FieldSelected event
    /// </summary>
    public class FieldSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the selected position.
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Gets the selected piece (null if the selected field is empty).
        /// </summary>
        public FriendlyPiece Piece { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="position">The selected field position.</param>
        /// <param name="piece">The selected piece (null if the selected field is empty)</param>
        public FieldSelectedEventArgs(Position position, FriendlyPiece piece)
        {
            Position = position;
            Piece = piece;
        }
    }
}
