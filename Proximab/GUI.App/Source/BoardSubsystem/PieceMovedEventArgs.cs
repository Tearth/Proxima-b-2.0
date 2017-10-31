using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Positions;

namespace GUI.App.Source.BoardSubsystem
{
    internal class PieceMovedEventArgs
    {
        public FriendlyPiece Piece { get; private set; }
        public Position From { get; private set; }
        public Position To { get; private set; }

        public PieceMovedEventArgs(FriendlyPiece piece, Position from, Position to)
        {
            Piece = piece;
            From = from;
            To = to;
        }
    }
}
