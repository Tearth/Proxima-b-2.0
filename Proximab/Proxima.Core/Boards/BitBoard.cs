using Proxima.Core.Boards.MoveGenerators;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Positions;
using System;
using System.Collections.Generic;

namespace Proxima.Core.Boards
{
    public class BitBoard
    {
        ulong[,] _pieces;
        ulong[] _occupancy;
        ulong[] _enPassant;

        ulong[] _attacks;
        ulong[] _attacksSummary;

        LinkedList<Move> _moves;

        CastlingData _castlingData;

        KnightMovesGenerator _knightMovesGenerator;
        KingMovesGenerator _kingMovesGenerator;
        RookMovesGenerator _rookMovesGenerator;
        BishopMovesGenerator _bishopMovesGenerator;
        PawnMovesGenerator _pawnMovesGenerator;

        public BitBoard()
        {
            _pieces = new ulong[2, 6];
            _occupancy = new ulong[2];
            _enPassant = new ulong[2];

            _attacksSummary = new ulong[2];
            _attacks = new ulong[64];

            _moves = new LinkedList<Move>();
            _castlingData = new CastlingData();

            _knightMovesGenerator = new KnightMovesGenerator();
            _kingMovesGenerator = new KingMovesGenerator();
            _rookMovesGenerator = new RookMovesGenerator();
            _bishopMovesGenerator = new BishopMovesGenerator();
            _pawnMovesGenerator = new PawnMovesGenerator();
        }

        public BitBoard(BitBoard bitBoard, Move move) : this()
        {
            Array.Copy(bitBoard._pieces, _pieces, 12);

            _castlingData = new CastlingData(bitBoard._castlingData);

            CalculateMove(bitBoard, move);
            CalculateEnPassant(move);
        }

        public BitBoard(FriendlyBoard friendlyBoard) : this()
        {
            ConvertFromFriendlyBoard(friendlyBoard);
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
                        var bitIndex = BitOperations.GetBitIndex(lsb);
                        var position = BitPositionConverter.ToPosition(bitIndex);

                        friendlyBoard.SetPiece(position, new FriendlyPiece((PieceType)i, (Color)c));
                    }
                }
            }

            _castlingData = new CastlingData(friendlyBoard.CastlingData);

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

        public bool[,] GetAttacks()
        {
            var attacks = _attacksSummary[(int)Color.White] | _attacksSummary[(int)Color.Black];
            return BitPositionConverter.ToBoolArray(attacks);
        }

        public bool[,] GetAttacks(Color color)
        {
            return BitPositionConverter.ToBoolArray(_attacksSummary[(int)color]);
        }

        public bool[,] GetFieldAttackers(Position position)
        {
            var bitIndex = BitPositionConverter.ToBitIndex(position);
            var array = 0ul;

            for(int piece=0; piece < 6; piece++)
            {
                array |= _attacks[bitIndex] | _attacks[bitIndex];
            }

            return BitPositionConverter.ToBoolArray(array);
        }

        public bool[,] GetFieldAttackers(Position position, Color color)
        {
            var bitIndex = BitPositionConverter.ToBitIndex(position);
            var array = 0ul;

            for (int piece = 0; piece < 6; piece++)
            {
                array |= _attacks[bitIndex];
            }

            return BitPositionConverter.ToBoolArray(array);
        }

        public LinkedList<Move> GetAvailableMoves()
        {
            return _moves;
        }

        public bool IsCheck(Color color)
        {
            var enemyColor = ColorOperations.Invert(color);
            var king = _pieces[(int)color, (int)PieceType.King];

            return (_attacksSummary[(int)enemyColor] & king) != 0;
        }

        public void Calculate(CalculationMode calculationMode)
        {
            CalculateOccupancy();
            CalculateAvailableMoves(calculationMode);
        }

        void CalculateMove(BitBoard bitBoard, Move move)
        {
            var colorIndex = (int)move.Color;
            var enemyColorIndex = (int)ColorOperations.Invert(move.Color);

            var pieceIndex = (int)move.Piece;
            
            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);

            _pieces[colorIndex, pieceIndex] &= ~from;

            if(move.Type == MoveType.Quiet)
            {

            }
            else if (move.Type == MoveType.Kill)
            {
                for (int i = 0; i < 6; i++)
                {
                    _pieces[enemyColorIndex, i] &= ~to;
                }
            }
            else if (move.Type == MoveType.EnPassant)
            {
                if (move.Color == Color.White)
                {
                    _pieces[enemyColorIndex, pieceIndex] &= ~(to >> 8);
                }
                else
                {
                    _pieces[enemyColorIndex, pieceIndex] &= ~(to << 8);
                }
            }
            else if (move.Type == MoveType.ShortCastling)
            {
                if(move.Color == Color.White)
                {
                    _pieces[(int)move.Color, (int)PieceType.Rook] &= ~KingMovesGenerator.WhiteRightRookLSB;
                    _pieces[(int)move.Color, (int)PieceType.Rook] |= (KingMovesGenerator.WhiteRightRookLSB << 2);

                    _castlingData.WhiteShortCastlingPossible = false;
                }
                else
                {
                    _pieces[(int)move.Color, (int)PieceType.Rook] &= ~KingMovesGenerator.BlackRightRookLSB;
                    _pieces[(int)move.Color, (int)PieceType.Rook] |= (KingMovesGenerator.BlackRightRookLSB << 2);

                    _castlingData.BlackShortCastlingPossible = false;
                }
            }
            else if (move.Type == MoveType.LongCastling)
            {
                if (move.Color == Color.White)
                {
                    _pieces[(int)move.Color, (int)PieceType.Rook] &= ~KingMovesGenerator.WhiteLeftRookLSB;
                    _pieces[(int)move.Color, (int)PieceType.Rook] |= (KingMovesGenerator.WhiteLeftRookLSB >> 3);

                    _castlingData.WhiteLongCastlingPossible = false;
                }
                else
                {
                    _pieces[(int)move.Color, (int)PieceType.Rook] &= ~KingMovesGenerator.BlackLeftRookLSB;
                    _pieces[(int)move.Color, (int)PieceType.Rook] |= (KingMovesGenerator.BlackLeftRookLSB >> 3);

                    _castlingData.BlackLongCastlingPossible = false;
                }
            }

            _pieces[colorIndex, pieceIndex] |= to;
        }

        void CalculateEnPassant(Move move)
        {
            if(move.Piece == PieceType.Pawn)
            {
                if(move.Color == Color.White)
                {
                    if(move.From.Y == 2 && move.To.Y == 4)
                    {
                        var enPassantPosition = new Position(move.To.X, move.To.Y - 1);
                        var enPassantLSB = BitPositionConverter.ToULong(enPassantPosition);

                        _enPassant[(int)Color.White] |= enPassantLSB;
                    }
                }
                else
                {
                    if (move.From.Y == 7 && move.To.Y == 5)
                    {
                        var enPassantPosition = new Position(move.To.X, move.To.Y + 1);
                        var enPassantLSB = BitPositionConverter.ToULong(enPassantPosition);

                        _enPassant[(int)Color.Black] |= enPassantLSB;
                    }
                }
            }
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

        void CalculateOccupancy()
        {
            for(int i=0; i<6; i++)
            {
                _occupancy[(int)Color.White] |= _pieces[(int)Color.White, i];
                _occupancy[(int)Color.Black] |= _pieces[(int)Color.Black, i];
            }
        }

        void CalculateAvailableMoves(CalculationMode calculationMode)
        {
            var whiteGeneratorModeFlags = GeneratorMode.None;
            var blackGeneratorModeFlags = GeneratorMode.None;

            if(calculationMode == CalculationMode.All)
            {
                whiteGeneratorModeFlags = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
                blackGeneratorModeFlags = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            }
            else if(calculationMode == CalculationMode.WhiteMovesPlusAttacks)
            {
                whiteGeneratorModeFlags = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
                blackGeneratorModeFlags = GeneratorMode.CalculateAttacks;
            }
            else if(calculationMode == CalculationMode.BlackMovesPlusAttacks)
            {
                whiteGeneratorModeFlags = GeneratorMode.CalculateAttacks;
                blackGeneratorModeFlags = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            }
            else if(calculationMode == CalculationMode.OnlyAttacks)
            {
                whiteGeneratorModeFlags = GeneratorMode.CalculateAttacks;
                blackGeneratorModeFlags = GeneratorMode.CalculateAttacks;
            }

            var whiteGeneratorParameters = GetGeneratorParameters(Color.White, whiteGeneratorModeFlags);
            var blackGeneratorParameters = GetGeneratorParameters(Color.Black, blackGeneratorModeFlags);

            CalculateAvailableMoves(whiteGeneratorParameters);
            CalculateAvailableMoves(blackGeneratorParameters);

            CalculateCastling(whiteGeneratorParameters);
            CalculateCastling(blackGeneratorParameters);
        }

        void CalculateAvailableMoves(GeneratorParameters generatorParameters)
        {
            _knightMovesGenerator.GetMoves(PieceType.Knight, generatorParameters);
            _kingMovesGenerator.GetMoves(PieceType.King, generatorParameters);
            _rookMovesGenerator.GetMoves(PieceType.Rook, generatorParameters);
            _bishopMovesGenerator.GetMoves(PieceType.Bishop, generatorParameters);
            _pawnMovesGenerator.GetMoves(PieceType.Pawn, generatorParameters);

            _rookMovesGenerator.GetMoves(PieceType.Queen, generatorParameters);
            _bishopMovesGenerator.GetMoves(PieceType.Queen, generatorParameters);
        }

        void CalculateCastling(GeneratorParameters generatorParameters)
        {
            if ((generatorParameters.Mode & GeneratorMode.CalculateMoves) == 0)
                return;

            _kingMovesGenerator.GetCastling(PieceType.King, generatorParameters);
        }

        GeneratorParameters GetGeneratorParameters(Color color, GeneratorMode mode)
        {
            return new GeneratorParameters()
            {
                FriendlyColor = color,
                EnemyColor = ColorOperations.Invert(color),

                Mode = mode,
                CastlingData = _castlingData,

                Occupancy = _occupancy[(int)Color.White] | _occupancy[(int)Color.Black],
                FriendlyOccupancy = _occupancy[(int)color],
                EnemyOccupancy = _occupancy[(int)ColorOperations.Invert(color)],

                Pieces = _pieces,
                EnPassant = _enPassant,

                Attacks = _attacks,
                AttacksSummary = _attacksSummary,

                Moves = _moves
            };
        }
    }
}
