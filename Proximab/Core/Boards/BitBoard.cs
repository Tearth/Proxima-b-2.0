using Core.Boards.MoveParsers;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Boards
{
    public class BitBoard
    {
        ulong[,] _pieces;
        ulong[] _occupancy;
        ulong[,] _attacks;

        List<Move> _whiteMoves;
        List<Move> _blackMoves;

        KnightMovesParser _knightMovesParser;
        KingMovesParser _kingMovesParser;
        RookMovesParser _rookMovesParser;
        BishopMovesParser _bishopMovesParser;
        PawnMovesParser _pawnMovesParser;

        public BitBoard()
        {
            _pieces = new ulong[2, 6];
            _occupancy = new ulong[2];
            _attacks = new ulong[2, 64];

            _whiteMoves = new List<Move>();
            _blackMoves = new List<Move>();

            _knightMovesParser = new KnightMovesParser();
            _kingMovesParser = new KingMovesParser();
            _rookMovesParser = new RookMovesParser();
            _bishopMovesParser = new BishopMovesParser();
            _pawnMovesParser = new PawnMovesParser();
        }

        public BitBoard(BitBoard bitBoard) : this()
        {
            Array.Copy(bitBoard._pieces, _pieces, 12);
        }

        public BitBoard(FriendlyBoard friendlyBoard) : this()
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    var position = new Position(x, y);
                    var piece = friendlyBoard.GetPiece(position);

                    if (piece != null)
                    {
                        var bitPosition = BitPositionConverter.ToULong(position);
                        _pieces[(int)piece.Color, (int)piece.Type] |= bitPosition;
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

            bitBoard._pieces[colorIndex, pieceIndex] &= ~from;

            for(int i=0; i<6; i++)
            {
                bitBoard._pieces[colorIndex, i] &= ~to;
            }

            bitBoard._pieces[colorIndex, pieceIndex] |= to;

            return bitBoard;
        }
        
        public FriendlyBoard GetFriendlyBoard()
        {
            var friendlyBoard = new FriendlyBoard();

            for(int c=0; c<2; c++)
            {
                for (int i = 0; i < 6; i++)
                {
                    var pieceArray = _pieces[c, i];

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
            var allOccupancy = _occupancy[(int)Color.White] | _occupancy[(int)Color.Black];
            return BitPositionConverter.ToBoolArray(allOccupancy);
        }

        public bool[,] GetFriendlyOccupancy(Color color)
        {
            return BitPositionConverter.ToBoolArray(_occupancy[(int)color]);
        }

        public bool[,] GetFieldAttackers(Position position)
        {
            var bitIndex = BitPositionConverter.ToBitIndex(position);
            var array = _attacks[(int)Color.White, bitIndex] | _attacks[(int)Color.Black, bitIndex];

            return BitPositionConverter.ToBoolArray(array);
        }

        public bool[,] GetFieldAttackers(Position position, Color color)
        {
            var bitIndex = BitPositionConverter.ToBitIndex(position);
            var array = _attacks[(int)color, bitIndex];

            return BitPositionConverter.ToBoolArray(array);
        }

        public List<Move> GetAvailableMoves()
        {
            var moves = new List<Move>();
            moves.AddRange(_whiteMoves);
            moves.AddRange(_blackMoves);

            return moves;
        }

        public List<Move> GetAvailableMoves(Color color)
        {
            return color == Color.White ? _whiteMoves : _blackMoves;
        }

        void CalculateOccupancy()
        {
            _occupancy[(int)Color.White] = _pieces[(int)Color.White, (int)PieceType.Pawn] |
                                          _pieces[(int)Color.White, (int)PieceType.Rook] |
                                          _pieces[(int)Color.White, (int)PieceType.Knight] |
                                          _pieces[(int)Color.White, (int)PieceType.Bishop] |
                                          _pieces[(int)Color.White, (int)PieceType.Queen] |
                                          _pieces[(int)Color.White, (int)PieceType.King];


            _occupancy[(int)Color.Black] = _pieces[(int)Color.Black, (int)PieceType.Pawn] |
                                          _pieces[(int)Color.Black, (int)PieceType.Rook] |
                                          _pieces[(int)Color.Black, (int)PieceType.Knight] |
                                          _pieces[(int)Color.Black, (int)PieceType.Bishop] |
                                          _pieces[(int)Color.Black, (int)PieceType.Queen] |
                                          _pieces[(int)Color.Black, (int)PieceType.King];
        }

        void CalculateMoves()
        {
            CalculateMoves(Color.White);
            CalculateMoves(Color.Black);
        }

        void CalculateMoves(Color color)
        {
            var occupancyContainer = new OccupancyContainer(color, _occupancy);
            var movesContainer = GetAvailableMoves(color);

            movesContainer.AddRange(_knightMovesParser.GetMoves(PieceType.Knight, color, _pieces, occupancyContainer, ref _attacks));
            movesContainer.AddRange(_kingMovesParser.GetMoves(PieceType.King, color, _pieces, occupancyContainer, ref _attacks));
            movesContainer.AddRange(_rookMovesParser.GetMoves(PieceType.Rook, color, _pieces, occupancyContainer, ref _attacks));
            movesContainer.AddRange(_bishopMovesParser.GetMoves(PieceType.Bishop, color, _pieces, occupancyContainer, ref _attacks));
            movesContainer.AddRange(_pawnMovesParser.GetMoves(PieceType.Pawn, color, _pieces, occupancyContainer, ref _attacks));

            movesContainer.AddRange(_rookMovesParser.GetMoves(PieceType.Queen, color, _pieces, occupancyContainer, ref _attacks));
            movesContainer.AddRange(_bishopMovesParser.GetMoves(PieceType.Queen, color, _pieces, occupancyContainer, ref _attacks));
        }
    }
}
