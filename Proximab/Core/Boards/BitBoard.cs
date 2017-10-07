using Core.Boards.MoveParsers;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using System.Collections.Generic;
using System.Linq;

namespace Core.Boards
{
    public class BitBoard
    {
        public ulong[,] Pieces { get; set; }
        public ulong[] Occupancy { get; set; }
        public ulong[,] Attacks { get; set; }

        List<Move> _moves;

        KnightMovesParser _knightMovesParser;
        KingMovesParser _kingMovesParser;

        public BitBoard()
        {
            Pieces = new ulong[2, 6];
            Occupancy = new ulong[2];
            Attacks = new ulong[2, 64];

            _moves = new List<Move>();

            _knightMovesParser = new KnightMovesParser(this);
            _kingMovesParser = new KingMovesParser(this);
        }

        public BitBoard(BitBoard bitBoard) : this()
        {
            Pieces = bitBoard.Pieces;
        }

        public BitBoard(FriendlyBoard friendlyBoard) : this()
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    var position = new Position(x, y);
                    var piece = friendlyBoard.GetPiece(position);

                    if (piece.Type != PieceType.None)
                    {
                        var bitPosition = BitPositionConverter.ToULong(position);
                        Pieces[(int)piece.Color, (int)piece.Type] |= bitPosition;
                    }
                }
            }
        }

        public void Calculate()
        {
            CalculateOccupancy();
            CalculateMoves();
        }

        public BitBoard Move(Move move)
        {
            var bitBoard = new BitBoard(this);

            var colorIndex = (int)move.Color;
            var pieceIndex = (int)move.Piece;

            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);

            bitBoard.Pieces[colorIndex, pieceIndex] &= ~from;

            for(int i=0; i<6; i++)
            {
                bitBoard.Pieces[colorIndex, i] &= ~to;
            }

            bitBoard.Pieces[colorIndex, pieceIndex] |= to;

            return bitBoard;
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

        public bool[,] GetFieldAttackers(Position position)
        {
            var bitIndex = BitPositionConverter.ToBitIndex(position);
            var array = Attacks[(int)Color.White, bitIndex] | Attacks[(int)Color.Black, bitIndex];

            return BitPositionConverter.ToBoolArray(array);
        }

        public bool[,] GetFieldAttackers(Position position, Color color)
        {
            var bitIndex = BitPositionConverter.ToBitIndex(position);
            var array = Attacks[(int)color, bitIndex];

            return BitPositionConverter.ToBoolArray(array);
        }

        public List<Move> GetAvailableMoves(Color color)
        {
            return _moves.Where(p => p.Color == color).ToList();
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

        void CalculateMoves()
        {
            CalculateMoves(Color.White);
            CalculateMoves(Color.Black);
        }

        void CalculateMoves(Color color)
        {
            _moves.AddRange(_knightMovesParser.GetMoves(color));
            _moves.AddRange(_kingMovesParser.GetMoves(color));
        }
    }
}
