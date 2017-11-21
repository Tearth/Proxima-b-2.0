using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Positions;
using System;

namespace GUI.App.Source.BoardSubsystem
{
    /// <summary>
    /// Represents information about FieldSelected event
    /// </summary>
    internal class FieldSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the selected position.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// Gets the selected piece (null in empty field).
        /// </summary>
        public FriendlyPiece Piece { get; private set; }

        public FieldSelectedEventArgs(Position position, FriendlyPiece piece)
        {
            Position = position;
            Piece = piece;
        }
    }
}
