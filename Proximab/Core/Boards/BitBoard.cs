using Core.Boards.MoveParsers;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using System.Collections.Generic;

namespace Core.Boards
{
    public class BitBoard
    {
        public ulong[] Pieces { get; set; }
        public ulong[] Occupancy { get; set; }

        KnightMovesParser _knightMovesParser;

        public BitBoard()
        {
            Pieces = new ulong[12];
            Occupancy = new ulong[2];

            _knightMovesParser = new KnightMovesParser(this);

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
                        Pieces[(int)piece] |= bitPosition;
                    }
                }
            }

            CalculateOccupancy();
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

                    var piece = (PieceType)i;

                    friendlyBoard.SetPiece(position, piece);
                }
            }

            return friendlyBoard;
        }

        public bool[,] GetFriendlyOccupancy()
        {
            var allOccupancy = Occupancy[(int)Color.White] | Occupancy[(int)Color.Black];
            return BitPositionConverter.ToBoolArray(allOccupancy);
        }

        public bool[,] GetFriendlyOccupancy(Color color)
        {
            return BitPositionConverter.ToBoolArray(Occupancy[(int)color]);
        }

        public List<Move> GetAvailableMoves(Color color)
        {
            return _knightMovesParser.GetMoves(color);
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
            Occupancy[(int)Color.White] = Pieces[(int)PieceType.WhitePawn] |
                                          Pieces[(int)PieceType.WhiteRook] |
                                          Pieces[(int)PieceType.WhiteKnight] |
                                          Pieces[(int)PieceType.WhiteBishop] |
                                          Pieces[(int)PieceType.WhiteQueen] |
                                          Pieces[(int)PieceType.WhiteKing];


            Occupancy[(int)Color.Black] = Pieces[(int)PieceType.BlackPawn] |
                                          Pieces[(int)PieceType.BlackRook] |
                                          Pieces[(int)PieceType.BlackKnight] |
                                          Pieces[(int)PieceType.BlackBishop] |
                                          Pieces[(int)PieceType.BlackQueen] |
                                          Pieces[(int)PieceType.BlackKing];
        }
    }
}
