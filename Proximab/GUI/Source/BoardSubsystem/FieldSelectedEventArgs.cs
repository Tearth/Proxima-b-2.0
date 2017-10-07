using Core.Boards;
using Core.Commons;
using Core.Commons.Positions;
using System;

namespace GUI.Source.BoardSubsystem
{
    internal class FieldSelectedEventArgs : EventArgs
    {
        public Position Position { get; set; }
        public FriendlyPiece Piece { get; set; }

        public FieldSelectedEventArgs(Position position, FriendlyPiece piece)
        {
            Position = position;
            Piece = piece;
        }
    }
}
