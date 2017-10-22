using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards
{
    public class FriendlyBoard
    {
        public bool[] Castling { get; set; }

        FriendlyPiece[,] _friendlyPieces;

        public FriendlyBoard()
        {
            _friendlyPieces = new FriendlyPiece[8, 8];
            Castling = new bool[4];

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    _friendlyPieces[x, y] = null;
                }
            }
        }

        public FriendlyPiece GetPiece(Position position)
        {
            return _friendlyPieces[position.X - 1, position.Y - 1];
        }

        public void SetPiece(Position position, FriendlyPiece piece)
        {
            _friendlyPieces[position.X - 1, position.Y - 1] = piece;
        }

        public void SetDefault()
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
            Castling[0] = true;
            Castling[1] = true;
            Castling[2] = true;
            Castling[3] = true;
        }
    }
}
