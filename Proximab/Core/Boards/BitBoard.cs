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
        public ulong[,] Pieces { get; set; }
        public ulong[] Occupancy { get; set; }

        KnightMovesParser _knightMovesParser;
        KingMovesParser _kingMovesParser;

        public BitBoard()
        {
            Pieces = new ulong[2, 6];
            Occupancy = new ulong[2];

            _knightMovesParser = new KnightMovesParser(this);
            _kingMovesParser = new KingMovesParser(this);

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

                    if(piece.Type != PieceType.None)
                    {
                        var bitPosition = BitPositionConverter.ToULong(position);
                        Pieces[(int)piece.Color, (int)piece.Type] |= bitPosition;
                    }
                }
            }

            CalculateOccupancy();
        }

        public FriendlyBoard GetFriendlyBoard()
        {
            var friendlyBoard = new FriendlyBoard();

            for(int c=0; c<2; c++)
            {
                for (int i = 0; i < 6; i++)
                {
                    var pieceArray = Pieces[c, i];

                    while (pieceArray != 0)
                    {
                        var lsb = BitOperations.GetLSB(ref pieceArray);
                        var position = BitPositionConverter.ToPosition(lsb);

                        friendlyBoard.SetPiece(position, new FriendlyPiece((PieceType)i, (Color)c));
                    }
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
            var availableMoves = new List<Move>();
            availableMoves.AddRange(_knightMovesParser.GetMoves(color));
            availableMoves.AddRange(_kingMovesParser.GetMoves(color));

            return availableMoves;
        }

        public void Clear()
        {
            for(int c = 0; c < 2; c++)
            {
                for (int i = 0; i < 6; i++)
                {
                    Pieces[c, i] = 0;
                }
            }
        }

        void CalculateOccupancy()
        {
            Occupancy[(int)Color.White] = Pieces[(int)Color.White, (int)PieceType.Pawn] |
                                          Pieces[(int)Color.White, (int)PieceType.Rook] |
                                          Pieces[(int)Color.White, (int)PieceType.Knight] |
                                          Pieces[(int)Color.White, (int)PieceType.Bishop] |
                                          Pieces[(int)Color.White, (int)PieceType.Queen] |
                                          Pieces[(int)Color.White, (int)PieceType.King];


            Occupancy[(int)Color.Black] = Pieces[(int)Color.Black, (int)PieceType.Pawn] |
                                          Pieces[(int)Color.Black, (int)PieceType.Rook] |
                                          Pieces[(int)Color.Black, (int)PieceType.Knight] |
                                          Pieces[(int)Color.Black, (int)PieceType.Bishop] |
                                          Pieces[(int)Color.Black, (int)PieceType.Queen] |
                                          Pieces[(int)Color.Black, (int)PieceType.King];
        }
    }
}
