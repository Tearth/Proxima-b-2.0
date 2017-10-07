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

            _knightMovesParser = new KnightMovesParser();

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
                        Pieces[(int)piece - 1] |= bitPosition;
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

                    var piece = (PieceType)(i + 1);

                    friendlyBoard.SetPiece(position, piece);
                }
            }

            return friendlyBoard;
        }

        public bool[,] GetFriendlyOccupancy()
        {
            var allOccupancy = Occupancy[0] | Occupancy[1];
            return BitPositionConverter.ToBoolArray(allOccupancy);
        }

        public bool[,] GetFriendlyOccupancy(Color color)
        {
            return BitPositionConverter.ToBoolArray(Occupancy[(int)color - 1]);
        }

        public List<Move> GetAvailableMoves(Color color)
        {
            return _knightMovesParser.GetMoves(this, color);
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
            Occupancy[0] = Pieces[(int)PieceType.WhitePawn - 1] |
                           Pieces[(int)PieceType.WhiteRook - 1] |
                           Pieces[(int)PieceType.WhiteKnight - 1] |
                           Pieces[(int)PieceType.WhiteBishop - 1] |
                           Pieces[(int)PieceType.WhiteQueen - 1] |
                           Pieces[(int)PieceType.WhiteKing - 1];


            Occupancy[1] = Pieces[(int)PieceType.BlackPawn - 1] |
                           Pieces[(int)PieceType.BlackRook - 1] |
                           Pieces[(int)PieceType.BlackKnight - 1] |
                           Pieces[(int)PieceType.BlackBishop - 1] |
                           Pieces[(int)PieceType.BlackQueen - 1] |
                           Pieces[(int)PieceType.BlackKing - 1];
        }
    }
}
