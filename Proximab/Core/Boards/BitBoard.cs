using Core.Commons;

namespace Core.Boards
{
    public class BitBoard
    {
        ulong[] _pieces;

        public BitBoard()
        {
            _pieces = new ulong[12];

            for(int i=0; i<_pieces.Length; i++)
            {
                _pieces[i] = 0;
            }
        }

        public void SyncWithFriendlyBoard(FriendlyBoard friendlyBoard)
        {
            var bitPositionConverter = new BitPositionConverter();

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
    }
}
