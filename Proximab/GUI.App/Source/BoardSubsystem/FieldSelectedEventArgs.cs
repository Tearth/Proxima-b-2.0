using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Positions;
using System;

namespace GUI.App.Source.BoardSubsystem
{
    internal class FieldSelectedEventArgs : EventArgs
    {
        public Position Position { get; private set; }
        public FriendlyPiece Piece { get; private set; }

        public FieldSelectedEventArgs(Position position, FriendlyPiece piece)
        {
            Position = position;
            Piece = piece;
        }
    }
}
