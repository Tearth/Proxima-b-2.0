using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    public class DefaultFriendlyBoard : FriendlyBoard
    {
        public DefaultFriendlyBoard() : base()
        {
            //White pieces
            SetPiece(new Position(1, 1), new FriendlyPiece(PieceType.Rook, Color.White));
            SetPiece(new Position(2, 1), new FriendlyPiece(PieceType.Knight, Color.White));
            SetPiece(new Position(3, 1), new FriendlyPiece(PieceType.Bishop, Color.White));
            SetPiece(new Position(4, 1), new FriendlyPiece(PieceType.Queen, Color.White));
            SetPiece(new Position(5, 1), new FriendlyPiece(PieceType.King, Color.White));
            SetPiece(new Position(6, 1), new FriendlyPiece(PieceType.Bishop, Color.White));
            SetPiece(new Position(7, 1), new FriendlyPiece(PieceType.Knight, Color.White));
            SetPiece(new Position(8, 1), new FriendlyPiece(PieceType.Rook, Color.White));

            SetPiece(new Position(1, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(2, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(3, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(4, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(5, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(6, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(7, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(8, 2), new FriendlyPiece(PieceType.Pawn, Color.White));

            //Black pieces
            SetPiece(new Position(1, 8), new FriendlyPiece(PieceType.Rook, Color.Black));
            SetPiece(new Position(2, 8), new FriendlyPiece(PieceType.Knight, Color.Black));
            SetPiece(new Position(3, 8), new FriendlyPiece(PieceType.Bishop, Color.Black));
            SetPiece(new Position(4, 8), new FriendlyPiece(PieceType.Queen, Color.Black));
            SetPiece(new Position(5, 8), new FriendlyPiece(PieceType.King, Color.Black));
            SetPiece(new Position(6, 8), new FriendlyPiece(PieceType.Bishop, Color.Black));
            SetPiece(new Position(7, 8), new FriendlyPiece(PieceType.Knight, Color.Black));
            SetPiece(new Position(8, 8), new FriendlyPiece(PieceType.Rook, Color.Black));

            SetPiece(new Position(1, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(2, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(3, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(4, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(5, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(6, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(7, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(8, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));

            //Castling
            Castling[(int)Color.White, (int)CastlingType.Short] = true;
            Castling[(int)Color.White, (int)CastlingType.Long] = true;
            Castling[(int)Color.Black, (int)CastlingType.Short] = true;
            Castling[(int)Color.Black, (int)CastlingType.Long] = true;

            //EnPassant
            EnPassant[(int)Color.White] = new bool[8, 8];
            EnPassant[(int)Color.Black] = new bool[8, 8];
        }
    }
}
