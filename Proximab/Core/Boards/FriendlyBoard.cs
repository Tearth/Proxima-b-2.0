using Core.Commons;

namespace Core.Boards
{
    public class FriendlyBoard
    {
        PieceType[,] _board { get; set; }

        public FriendlyBoard()
        {
            _board = new PieceType[8, 8];

            for(int x=0; x<8; x++)
            {
                for(int y=0; y<8; y++)
                {
                    _board[x, y] = PieceType.None;
                }
            }
        }

        public PieceType GetPiece(Position position)
        {
            return _board[position.X - 1, position.Y - 1];
        }

        public void SetPiece(Position position, PieceType pieceType)
        {
            _board[position.X - 1, position.Y - 1] = pieceType;
        }
    }
}
