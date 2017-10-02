using Core.Common;
using System;

namespace GUI.Source.BoardSubsystem.Selections
{
    internal class FieldSelectedEventArgs : EventArgs
    {
        public Position Position { get; set; }
        public PieceType PieceType { get; set; }

        public FieldSelectedEventArgs(Position position, PieceType pieceType)
        {
            Position = position;
            PieceType = pieceType;
        }
    }
}
