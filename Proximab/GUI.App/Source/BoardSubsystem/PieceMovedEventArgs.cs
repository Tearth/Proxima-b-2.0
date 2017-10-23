using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Positions;

namespace GUI.App.Source.BoardSubsystem
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
