using Core.Commons;

namespace Core.Boards
{
    public class BitBoard
    {
        ulong[] _pieces;

        public BitBoard()
        {
            _pieces = new ulong[12];

            Clear();
        }

        public void SyncWithFriendlyBoard(FriendlyBoard friendlyBoard)
        {
            var bitPositionConverter = new BitPositionConverter();
            Clear();

            for(int x=1; x<=8; x++)
            {
                for(int y=1; y<=8; y++)
                {
                    var position = new Position(x, y);
                    var piece = friendlyBoard.GetPiece(position);

                    if(piece != PieceType.None)
                    {
                        var bitPosition = bitPositionConverter.Convert(position);
                        _pieces[(int)piece - 1] |= bitPosition;
                    }
                }
            }
        }

        public FriendlyBoard GetFriendlyBoard()
        {
            var friendlyBoard = new FriendlyBoard();
            var bitPositionConverter = new BitPositionConverter();

            for (int i=0; i<12; i++)
            {
                var pieceArray = _pieces[i];

                while(pieceArray != 0)
                {
                    var bitPosition = (ulong)((long)pieceArray & -((long)pieceArray));

                    var position = bitPositionConverter.Convert(bitPosition);
                    var piece = (PieceType)(i + 1);

                    friendlyBoard.SetPiece(position, piece);

                    pieceArray &= pieceArray - 1;
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
