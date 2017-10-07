using Core.Commons;
using Core.Commons.Positions;

namespace GUI.Source.BoardSubsystem
{
    internal class PieceMovedEventArgs
    {
        public PieceType PieceType;
        public Position From;
        public Position To;

        public PieceMovedEventArgs(PieceType pieceType, Position from, Position to)
        {
            PieceType = pieceType;
            From = from;
            To = to;
        }
    }
}
