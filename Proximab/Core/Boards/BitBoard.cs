using Core.Commons;

namespace Core.Boards
{
    public class BitBoard
    {
        ulong[] _pieces;

        ulong _whiteOccupancy
        {
            get
            {
                return _pieces[(int)PieceType.WhitePawn] | 
                       _pieces[(int)PieceType.WhiteRook] | 
                       _pieces[(int)PieceType.WhiteKnight] | 
                       _pieces[(int)PieceType.WhiteBishop] | 
                       _pieces[(int)PieceType.WhiteQueen] | 
                       _pieces[(int)PieceType.WhiteKing];
            }
        }

        ulong _blackOccupancy
        {
            get
            {
                return _pieces[(int)PieceType.BlackPawn] |
                       _pieces[(int)PieceType.BlackRook] |
                       _pieces[(int)PieceType.BlackKnight] |
                       _pieces[(int)PieceType.BlackBishop] |
                       _pieces[(int)PieceType.BlackQueen] |
                       _pieces[(int)PieceType.BlackKing];
            }
        }

        public BitBoard()
        {
            _pieces = new ulong[12];

            Clear();
        }

        public void SyncWithFriendlyBoard(FriendlyBoard friendlyBoard)
        {
            Clear();

            for(int x=1; x<=8; x++)
            {
                for(int y=1; y<=8; y++)
                {
                    var position = new Position(x, y);
                    var piece = friendlyBoard.GetPiece(position);

                    if(piece != PieceType.None)
                    {
                        var bitPosition = BitPositionConverter.ToULong(position);
                        _pieces[(int)piece - 1] |= bitPosition;
                    }
                }
            }
        }

        public FriendlyBoard GetFriendlyBoard()
        {
            var friendlyBoard = new FriendlyBoard();

            for (int i=0; i<12; i++)
            {
                var pieceArray = _pieces[i];

                while(pieceArray != 0)
                {
                    var bitPosition = BitOperations.GetLSB(ref pieceArray);
                    var position = BitPositionConverter.ToPosition(bitPosition);

                    var piece = (PieceType)(i + 1);

                    friendlyBoard.SetPiece(position, piece);
                }
            }

            return friendlyBoard;
        }

        void Clear()
        {
            for (int i = 0; i < _pieces.Length; i++)
            {
                _pieces[i] = 0;
            }
        }
    }
}
