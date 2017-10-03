using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.BoardSubsystem
{
    internal class PieceMoveEventArgs
    {
        public PieceType PieceType;
        public Position From;
        public Position To;

        public PieceMoveEventArgs(PieceType pieceType, Position from, Position to)
        {
            PieceType = pieceType;
            From = from;
            To = to;
        }
    }
}
