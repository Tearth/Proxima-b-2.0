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

            for(int x=0; x<8; x++)
            {
                for(int y=0; y<8; y++)
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
    }
}
