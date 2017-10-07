using Core.Boards;
using Core.Commons;
using Core.Commons.Positions;

namespace GUI.Source.BoardSubsystem
{
    internal class PieceMovedEventArgs
    {
        public FriendlyPiece Piece;
        public Position From;
        public Position To;

        public PieceMovedEventArgs(FriendlyPiece piece, Position from, Position to)
        {
            Piece = piece;
            From = from;
            To = to;
        }
    }
}
