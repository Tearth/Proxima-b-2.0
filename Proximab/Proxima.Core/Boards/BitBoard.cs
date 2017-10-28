using Proxima.Core.Boards.Friendly;
using Proxima.Core.Boards.MoveGenerators;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Exceptions;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Positions;
using System;
using System.Collections.Generic;

namespace Proxima.Core.Boards
{
    public class BitBoard
    {
        ulong[] _pieces;
        ulong[] _occupancy;
        ulong[] _enPassant;

        ulong[] _attacks;
        ulong[] _attacksSummary;

        bool[] _castling { get; set; }

        LinkedList<Move> _moves;

        KnightMovesGenerator _knightMovesGenerator;
        KingMovesGenerator _kingMovesGenerator;
        RookMovesGenerator _rookMovesGenerator;
        BishopMovesGenerator _bishopMovesGenerator;
        PawnMovesGenerator _pawnMovesGenerator;

        public BitBoard()
        {
            _pieces = new ulong[12];
            _occupancy = new ulong[2];
            _enPassant = new ulong[2];

            _attacksSummary = new ulong[2];
            _attacks = new ulong[64];

            _castling = new bool[4];

            _moves = new LinkedList<Move>();

            _knightMovesGenerator = new KnightMovesGenerator();
            _kingMovesGenerator = new KingMovesGenerator();
            _rookMovesGenerator = new RookMovesGenerator();
            _bishopMovesGenerator = new BishopMovesGenerator();
            _pawnMovesGenerator = new PawnMovesGenerator();
        }

        public BitBoard(BitBoard bitBoard, Move move) : this()
        {
            Buffer.BlockCopy(bitBoard._pieces, 0, _pieces, 0, bitBoard._pieces.Length * sizeof(ulong));
            Buffer.BlockCopy(bitBoard._castling, 0, _castling, 0, bitBoard._castling.Length * sizeof(bool));

            CalculateMove(bitBoard, move);
            CalculateEnPassant(move);
        }

        public BitBoard(FriendlyBoard friendlyBoard) : this()
        {
            _pieces = friendlyBoard.GetPiecesArray();
            _castling = friendlyBoard.GetCastlingArray();
            _enPassant = friendlyBoard.GetEnPassantArray();
        }

        public BitBoard Move(Move move)
        {
            return new BitBoard(this, move);
        }
        
        public FriendlyBoard GetFriendlyBoard()
        {
            return new FriendlyBoard(_pieces, _attacks, _castling, _enPassant);
        }

        public LinkedList<Move> GetAvailableMoves()
        {
            return _moves;
        }

        public bool IsCheck(Color color)
        {
            var enemyColor = ColorOperations.Invert(color);
            var king = _pieces[FastArray.GetPieceIndex(color, PieceType.King)];

            return (_attacksSummary[(int)enemyColor] & king) != 0;
        }

        public void Calculate(CalculationMode calculationMode)
        {
            CalculateOccupancy();
            CalculateAvailableMoves(calculationMode);
        }

        void CalculateMove(BitBoard bitBoard, Move move)
        {
            if (!move.IsValid())
            {
                throw new PositionOutOfRangeException();
            }
            
            var from = BitPositionConverter.ToULong(move.From);
            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] &= ~from;

            switch(move)
            {
                case QuietMove quietMove: { CalculateQuietMove(quietMove); break; }
                case KillMove killMove: { CalculateKillMove(killMove); break; }
                case EnPassantMove enPassantMove: { CalculateEnPassantMove(enPassantMove); break; }
                case CastlingMove castlingMove: { CalculateCastlingMove(castlingMove); break; }
                case PromotionMove promotionMove: { CalculatePromotionMove(promotionMove); break; }
            }
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

        void CalculateQuietMove(QuietMove move)
        {
            var to = BitPositionConverter.ToULong(move.To);

            if (move.Piece == PieceType.King)
            {
                _castling[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
                _castling[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;
            }
            else if (move.Piece == PieceType.Rook)
            {
                if (move.From == new Position(1, 1) || move.From == new Position(1, 8))
                {
                    _castling[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;
                }
                else if (move.From == new Position(8, 1) || move.From == new Position(8, 8))
                {
                    _castling[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
                }
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] |= to;
        }

        void CalculateKillMove(KillMove move)
        {
            var enemyColor = ColorOperations.Invert(move.Color);
            var to = BitPositionConverter.ToULong(move.To);

            for (int piece = 0; piece < 6; piece++)
            {
                _pieces[FastArray.GetPieceIndex(enemyColor, (PieceType)piece)] &= ~to;
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] |= to;
        }

        void CalculateCastlingMove(CastlingMove move)
        {
            var to = BitPositionConverter.ToULong(move.To);

            switch (move.CastlingType)
            {
                case CastlingType.Short:
                {
                    var rookLSB = move.Color == Color.White ? KingMovesGenerator.WhiteRightRookLSB : KingMovesGenerator.BlackRightRookLSB;

                    _pieces[FastArray.GetPieceIndex(move.Color, PieceType.Rook)] &= ~rookLSB;
                    _pieces[FastArray.GetPieceIndex(move.Color, PieceType.Rook)] |= (rookLSB << 2);

                    _castling[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
                    _castling[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;
                    break;
                }
                case CastlingType.Long:
                {
                    var rookLSB = move.Color == Color.White ? KingMovesGenerator.WhiteLeftRookLSB : KingMovesGenerator.BlackLeftRookLSB;

                    _pieces[FastArray.GetPieceIndex(move.Color, PieceType.Rook)] &= ~rookLSB;
                    _pieces[FastArray.GetPieceIndex(move.Color, PieceType.Rook)] |= (rookLSB >> 3);

                    _castling[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
                    _castling[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;
                    break;
                }
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] |= to;
        }

        void CalculateEnPassantMove(EnPassantMove move)
        {
            var enemyColor = ColorOperations.Invert(move.Color);
            var to = BitPositionConverter.ToULong(move.To);

            if (move.Color == Color.White)
            {
                _pieces[FastArray.GetPieceIndex(enemyColor, move.Piece)] &= ~(to >> 8);
            }
            else
            {
                _pieces[FastArray.GetPieceIndex(enemyColor, move.Piece)] &= ~(to << 8);
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] |= to;
        }

        void CalculatePromotionMove(PromotionMove move)
        {
            var to = BitPositionConverter.ToULong(move.To);

            _pieces[FastArray.GetPieceIndex(move.Color, move.PromotionPiece)] |= to;
        }

        void CalculateOccupancy()
        {
            for(int piece=0; piece<6; piece++)
            {
                _occupancy[(int)Color.White] |= _pieces[FastArray.GetPieceIndex(Color.White, (PieceType)piece)];
                _occupancy[(int)Color.Black] |= _pieces[FastArray.GetPieceIndex(Color.Black, (PieceType)piece)];
            }
        }

        void CalculateAvailableMoves(CalculationMode calculationMode)
        {
            GeneratorMode whiteGeneratorModeFlags = GeneratorMode.CalculateMoves;
            GeneratorMode blackGeneratorModeFlags = GeneratorMode.CalculateMoves;

            switch(calculationMode)
            {
                case CalculationMode.All:
                {
                    whiteGeneratorModeFlags = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
                    blackGeneratorModeFlags = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;

                    break;
                }
                case CalculationMode.WhiteMovesPlusAttacks:
                {
                    whiteGeneratorModeFlags = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
                    blackGeneratorModeFlags = GeneratorMode.CalculateAttacks;

                    break;
                }
                case CalculationMode.BlackMovesPlusAttacks:
                {
                    whiteGeneratorModeFlags = GeneratorMode.CalculateAttacks;
                    blackGeneratorModeFlags = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;

                    break;
                }
                case CalculationMode.OnlyAttacks:
                {
                    whiteGeneratorModeFlags = GeneratorMode.CalculateAttacks;
                    blackGeneratorModeFlags = GeneratorMode.CalculateAttacks;

                    break;
                }
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
            _pawnMovesGenerator.Calculate(generatorParameters);
            _knightMovesGenerator.Calculate(generatorParameters);
            _kingMovesGenerator.Calculate(generatorParameters);

            _rookMovesGenerator.Calculate(PieceType.Rook, generatorParameters);
            _bishopMovesGenerator.Calculate(PieceType.Bishop, generatorParameters);

            _rookMovesGenerator.Calculate(PieceType.Queen, generatorParameters);
            _bishopMovesGenerator.Calculate(PieceType.Queen, generatorParameters);
        }

        void CalculateCastling(GeneratorParameters generatorParameters)
        {
            if ((generatorParameters.Mode & GeneratorMode.CalculateMoves) == 0)
                return;

            _kingMovesGenerator.CalculateCastling(generatorParameters);
        }

        GeneratorParameters GetGeneratorParameters(Color color, GeneratorMode mode)
        {
            return new GeneratorParameters()
            {
                FriendlyColor = color,
                EnemyColor = ColorOperations.Invert(color),

                Mode = mode,
                Castling = _castling,

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
