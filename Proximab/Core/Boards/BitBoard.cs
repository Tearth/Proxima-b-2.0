using Core.Commons;

namespace Core.Boards
{
    public class BitBoard
    {
        ulong[] _pieces;

        ulong _occupancy
        {
            get
            {
                return _whiteOccupancy | _blackOccupancy;
            }
        }

        ulong _whiteOccupancy
        {
            get
            {
                return _pieces[(int)PieceType.WhitePawn - 1] | 
                       _pieces[(int)PieceType.WhiteRook - 1] | 
                       _pieces[(int)PieceType.WhiteKnight - 1] | 
                       _pieces[(int)PieceType.WhiteBishop - 1] | 
                       _pieces[(int)PieceType.WhiteQueen - 1] | 
                       _pieces[(int)PieceType.WhiteKing - 1];
            }
        }

        ulong _blackOccupancy
        {
            get
            {
                return _pieces[(int)PieceType.BlackPawn - 1] |
                       _pieces[(int)PieceType.BlackRook - 1] |
                       _pieces[(int)PieceType.BlackKnight - 1] |
                       _pieces[(int)PieceType.BlackBishop - 1] |
                       _pieces[(int)PieceType.BlackQueen - 1] |
                       _pieces[(int)PieceType.BlackKing - 1];
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
                    var lsb = BitOperations.GetLSB(ref pieceArray);
                    var position = BitPositionConverter.ToPosition(lsb);

                    var piece = (PieceType)(i + 1);

                    friendlyBoard.SetPiece(position, piece);
                }
            }

            return friendlyBoard;
        }

        public bool[,] GetOccupancy()
        {
            return BitPositionConverter.ToBoolArray(_occupancy);
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
