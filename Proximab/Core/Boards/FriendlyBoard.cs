using Core.Commons;
using Core.Commons.Positions;

namespace Core.Boards
{
    public class FriendlyBoard
    {
        FriendlyPiece[,] _board { get; set; }

        public FriendlyBoard()
        {
            _board = new FriendlyPiece[8, 8];

            for(int x=0; x<8; x++)
            {
                for(int y=0; y<8; y++)
                {
                    _board[x, y] = new FriendlyPiece();
                }
            }
        }

        public FriendlyPiece GetPiece(Position position)
        {
            return _board[position.X - 1, position.Y - 1];
        }

        public void SetPiece(Position position, FriendlyPiece piece)
        {
            _board[position.X - 1, position.Y - 1] = piece;
        }
    }
}
