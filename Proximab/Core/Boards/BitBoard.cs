using Core.Commons;

namespace Core.Boards
{
    public class BitBoard
    {
        public ulong[] Pieces { get; set; }
        public ulong[] PieceMoves { get; set; }

        public ulong Occupancy { get; set; }
        public ulong WhiteOccupancy { get; set; }
        public ulong BlackOccupancy { get; set; }

        public BitBoard()
        {
            Pieces = new ulong[12];
            PieceMoves = new ulong[12];

            Clear();
        }

        public void Calculate()
        {
            CalculateOccupancy();
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
                        Pieces[(int)piece - 1] |= bitPosition;
                    }
                }
            }
        }

        public FriendlyBoard GetFriendlyBoard()
        {
            var friendlyBoard = new FriendlyBoard();

            for (int i=0; i<12; i++)
            {
                var pieceArray = Pieces[i];

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

        public bool[,] GetFriendlyOccupancy()
        {
            return BitPositionConverter.ToBoolArray(Occupancy);
        }

        public bool[,] GetFriendlyOccupancy(Color color)
        {
            switch(color)
            {
                case Color.White: return BitPositionConverter.ToBoolArray(WhiteOccupancy);
                case Color.Black: return BitPositionConverter.ToBoolArray(BlackOccupancy);
            }

            return null;
        }

        public void Clear()
        {
            for (int i = 0; i < Pieces.Length; i++)
            {
                Pieces[i] = 0;
            }
        }

        void CalculateOccupancy()
        {
            WhiteOccupancy = Pieces[(int)PieceType.WhitePawn - 1] |
                             Pieces[(int)PieceType.WhiteRook - 1] |
                             Pieces[(int)PieceType.WhiteKnight - 1] |
                             Pieces[(int)PieceType.WhiteBishop - 1] |
                             Pieces[(int)PieceType.WhiteQueen - 1] |
                             Pieces[(int)PieceType.WhiteKing - 1];


            BlackOccupancy = Pieces[(int)PieceType.BlackPawn - 1] |
                             Pieces[(int)PieceType.BlackRook - 1] |
                             Pieces[(int)PieceType.BlackKnight - 1] |
                             Pieces[(int)PieceType.BlackBishop - 1] |
                             Pieces[(int)PieceType.BlackQueen - 1] |
                             Pieces[(int)PieceType.BlackKing - 1];

            Occupancy = WhiteOccupancy | BlackOccupancy;
        }
    }
}
