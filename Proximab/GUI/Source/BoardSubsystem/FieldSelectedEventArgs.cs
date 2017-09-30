using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.BoardSubsystem
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
