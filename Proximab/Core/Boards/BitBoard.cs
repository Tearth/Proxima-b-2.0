using Core.Boards.MoveParsers;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using System;
using System.Collections.Generic;

namespace Core.Boards
{
    public class BitBoard
    {
        ulong[,] _pieces;
        ulong[] _occupancy;
        ulong[,] _attacks;

        LinkedList<Move> _whiteMoves;
        LinkedList<Move> _blackMoves;

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

            _whiteMoves = new LinkedList<Move>();
            _blackMoves = new LinkedList<Move>();

            _knightMovesParser = new KnightMovesParser();
            _kingMovesParser = new KingMovesParser();
            _rookMovesParser = new RookMovesParser();
            _bishopMovesParser = new BishopMovesParser();
            _pawnMovesParser = new PawnMovesParser();
        }

        public BitBoard(BitBoard bitBoard, Move move) : this()
        {
            CalculateMove(bitBoard, move);
            CalculateBitBoard();
        }

        public BitBoard(FriendlyBoard friendlyBoard) : this()
        {
            ConvertFromFriendlyBoard(friendlyBoard);
            CalculateBitBoard();
        }

        public BitBoard Move(Move move)
        {
            return new BitBoard(this, move);
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

        public LinkedList<Move> GetAvailableMoves()
        {
            var moves = new LinkedList<Move>();

            foreach (var move in _whiteMoves)
                moves.AddLast(move);

            foreach (var move in _blackMoves)
                moves.AddLast(move);

            return moves;
        }

        public LinkedList<Move> GetAvailableMoves(Color color)
        {
            return color == Color.White ? _whiteMoves : _blackMoves;
        }

        public bool IsCheck(Color color)
        {
            var enemyColor = ColorOperations.Invert(color);
            var kingIndex = BitOperations.GetBitIndex(_pieces[(int)color, (int)PieceType.King]);

            return _attacks[(int)enemyColor, kingIndex] != 0;
        }

        void CalculateMove(BitBoard bitBoard, Move move)
        {
            Array.Copy(bitBoard._pieces, _pieces, 12);

            var colorIndex = (int)move.Color;
            var pieceIndex = (int)move.Piece;

            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);

            _pieces[colorIndex, pieceIndex] &= ~from;

            for (int i = 0; i < 6; i++)
            {
                _pieces[colorIndex, i] &= ~to;
            }

            _pieces[colorIndex, pieceIndex] |= to;
        }

        void ConvertFromFriendlyBoard(FriendlyBoard friendlyBoard)
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

        void CalculateBitBoard()
        {
            CalculateOccupancy();
            CalculateAvailableMoves();
        }

        void CalculateOccupancy()
        {
            for(int i=0; i<6; i++)
            {
                _occupancy[(int)Color.White] |= _pieces[(int)Color.White, i];
                _occupancy[(int)Color.Black] |= _pieces[(int)Color.Black, i];
            }
        }

        void CalculateAvailableMoves()
        {
            CalculateAvailableMoves(Color.White);
            CalculateAvailableMoves(Color.Black);
        }

        void CalculateAvailableMoves(Color color)
        {
            var occupancyContainer = new OccupancyContainer(color, _occupancy);
            var movesContainer = GetAvailableMoves(color);

            _knightMovesParser.GetMoves(PieceType.Knight, color, _pieces, occupancyContainer, movesContainer, ref _attacks);
            _kingMovesParser.GetMoves(PieceType.King, color, _pieces, occupancyContainer, movesContainer, ref _attacks);
            _rookMovesParser.GetMoves(PieceType.Rook, color, _pieces, occupancyContainer, movesContainer, ref _attacks);
            _bishopMovesParser.GetMoves(PieceType.Bishop, color, _pieces, occupancyContainer, movesContainer, ref _attacks);
            _pawnMovesParser.GetMoves(PieceType.Pawn, color, _pieces, occupancyContainer, movesContainer, ref _attacks);

            _rookMovesParser.GetMoves(PieceType.Queen, color, _pieces, occupancyContainer, movesContainer, ref _attacks);
            _bishopMovesParser.GetMoves(PieceType.Queen, color, _pieces, occupancyContainer, movesContainer, ref _attacks);
        }
    }
}
