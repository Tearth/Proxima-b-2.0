using Proxima.Core.Boards.Friendly;
using Proxima.Core.MoveGenerators;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Exceptions;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Positions;
using System;
using System.Collections.Generic;
using Proxima.Core.Evaluation;
using Proxima.Core.Boards.Hashing;

namespace Proxima.Core.Boards
{
    public class BitBoard
    {
        ulong[] _pieces;
        ulong[] _occupancy;
        ulong[] _enPassant;

        ulong[] _attacks;
        ulong[] _attacksSummary;

        bool[] _castlingPossibility;
        bool[] _castlingDone;

        ulong _hash;

        LinkedList<Move> _moves;

        ZobristUpdater _zobristUpdater;
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

            _attacks = new ulong[64];
            _attacksSummary = new ulong[2];

            _castlingPossibility = new bool[4];
            _castlingDone = new bool[2];

            _moves = new LinkedList<Move>();

            _zobristUpdater = new ZobristUpdater();
            _knightMovesGenerator = new KnightMovesGenerator();
            _kingMovesGenerator = new KingMovesGenerator();
            _rookMovesGenerator = new RookMovesGenerator();
            _bishopMovesGenerator = new BishopMovesGenerator();
            _pawnMovesGenerator = new PawnMovesGenerator();
        }

        public BitBoard(BitBoard bitBoard, Move move) : this()
        {
            _hash = bitBoard._hash;

            Buffer.BlockCopy(bitBoard._pieces, 0, _pieces, 0, bitBoard._pieces.Length * sizeof(ulong));
            Buffer.BlockCopy(bitBoard._castlingPossibility, 0, _castlingPossibility, 0, bitBoard._castlingPossibility.Length * sizeof(bool));
            Buffer.BlockCopy(bitBoard._castlingDone, 0, _castlingDone, 0, bitBoard._castlingDone.Length * sizeof(bool));

            CalculateMove(bitBoard, move);
            CalculateEnPassant(move);
        }

        public BitBoard(FriendlyBoard friendlyBoard) : this()
        {
            _pieces = friendlyBoard.GetPiecesArray();
            _castlingPossibility = friendlyBoard.GetCastlingPossibilityArray();
            _castlingDone = friendlyBoard.GetCastlingDoneArray();
            _enPassant = friendlyBoard.GetEnPassantArray();

            _hash = GetHash(true);
        }

        public BitBoard Move(Move move)
        {
            return new BitBoard(this, move);
        }
        
        public FriendlyBoard GetFriendlyBoard()
        {
            return new FriendlyBoard(_pieces, _attacks, _castlingPossibility, _castlingDone, _enPassant);
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

        public void Calculate()
        {
            var generatorMode = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            Calculate(generatorMode, generatorMode);
        }

        public void Calculate(GeneratorMode whiteMode, GeneratorMode blackMode)
        {
            CalculateOccupancy();
            CalculateAvailableMoves(whiteMode, blackMode);
        }

        public EvaluationResult GetEvaluation()
        {
            var evaluationCalculator = new EvaluationCalculator();
            var evaluationParameters = GetEvaluationParameters();

            return evaluationCalculator.GetEvaluation(evaluationParameters);
        }

        public ulong GetHash(bool generateNew)
        {
            if(generateNew)
            {
                return new ZobristHash().Calculate(_pieces, _castlingPossibility, _enPassant);
            }

            return _hash;
        }

        void CalculateMove(BitBoard bitBoard, Move move)
        {
            var from = BitPositionConverter.ToULong(move.From);

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] &= ~from;
            _zobristUpdater.AddOrRemovePiece(ref _hash, move.Color, move.Piece, from);

            switch(move)
            {
                case QuietMove quietMove: { CalculateQuietMove(quietMove); break; }
                case KillMove killMove: { CalculateKillMove(killMove); break; }
                case EnPassantMove enPassantMove: { CalculateEnPassantMove(enPassantMove); break; }
                case CastlingMove castlingMove: { CalculateCastlingMove(castlingMove); break; }
                case PromotionMove promotionMove: { CalculatePromotionMove(promotionMove); break; }
            }
        }

        void CalculateQuietMove(QuietMove move)
        {
            var to = BitPositionConverter.ToULong(move.To);

            if (move.Piece == PieceType.King)
            {
                _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
                _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;

                _zobristUpdater.RemoveCastlingPossibility(ref _hash, move.Color, CastlingType.Short);
                _zobristUpdater.RemoveCastlingPossibility(ref _hash, move.Color, CastlingType.Long);
            }
            else if (move.Piece == PieceType.Rook)
            {
                if (move.From == new Position(1, 1) || move.From == new Position(1, 8))
                {
                    _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;
                    _zobristUpdater.RemoveCastlingPossibility(ref _hash, move.Color, CastlingType.Long);
                }
                else if (move.From == new Position(8, 1) || move.From == new Position(8, 8))
                {
                    _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
                    _zobristUpdater.RemoveCastlingPossibility(ref _hash, move.Color, CastlingType.Short);
                }
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] |= to;
            _zobristUpdater.AddOrRemovePiece(ref _hash, move.Color, move.Piece, to);
        }

        void CalculateKillMove(KillMove move)
        {
            var enemyColor = ColorOperations.Invert(move.Color);
            var to = BitPositionConverter.ToULong(move.To);

            for (int piece = 0; piece < 6; piece++)
            {
                var index = FastArray.GetPieceIndex(enemyColor, (PieceType)piece);
                if((_pieces[index] & to) != 0)
                {
                    _pieces[index] &= ~to;
                    _zobristUpdater.AddOrRemovePiece(ref _hash, enemyColor, (PieceType)piece, to);

                    break;
                }
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] |= to;
            _zobristUpdater.AddOrRemovePiece(ref _hash, move.Color, move.Piece, to);
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
                        
                    _zobristUpdater.AddOrRemovePiece(ref _hash, move.Color, PieceType.Rook, rookLSB);
                    _zobristUpdater.AddOrRemovePiece(ref _hash, move.Color, PieceType.Rook, rookLSB << 2);

                    break;
                }
                case CastlingType.Long:
                {
                    var rookLSB = move.Color == Color.White ? KingMovesGenerator.WhiteLeftRookLSB : KingMovesGenerator.BlackLeftRookLSB;

                    _pieces[FastArray.GetPieceIndex(move.Color, PieceType.Rook)] &= ~rookLSB;
                    _pieces[FastArray.GetPieceIndex(move.Color, PieceType.Rook)] |= (rookLSB >> 3);

                    _zobristUpdater.AddOrRemovePiece(ref _hash, move.Color, PieceType.Rook, rookLSB);
                    _zobristUpdater.AddOrRemovePiece(ref _hash, move.Color, PieceType.Rook, rookLSB >> 3);

                    break;
                }
            }

            _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
            _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;

            _zobristUpdater.RemoveCastlingPossibility(ref _hash, move.Color, CastlingType.Short);
            _zobristUpdater.RemoveCastlingPossibility(ref _hash, move.Color, CastlingType.Long);

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] |= to;
            _zobristUpdater.AddOrRemovePiece(ref _hash, move.Color, move.Piece, to);

            _castlingDone[(int)move.Color] = true;
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
            _zobristUpdater.AddOrRemovePiece(ref _hash, move.Color, move.PromotionPiece, to);
        }

        void CalculateEnPassant(Move move)
        {
            if (move.Piece == PieceType.Pawn)
            {
                if (move.Color == Color.White)
                {
                    if (move.From.Y == 2 && move.To.Y == 4)
                    {
                        var enPassantPosition = new Position(move.To.X, move.To.Y - 1);
                        var enPassantLSB = BitPositionConverter.ToULong(enPassantPosition);

                        _enPassant[(int)Color.White] |= enPassantLSB;
                        _zobristUpdater.AddEnPassant(ref _hash, Color.White, enPassantLSB);
                    }
                }
                else
                {
                    if (move.From.Y == 7 && move.To.Y == 5)
                    {
                        var enPassantPosition = new Position(move.To.X, move.To.Y + 1);
                        var enPassantLSB = BitPositionConverter.ToULong(enPassantPosition);

                        _enPassant[(int)Color.Black] |= enPassantLSB;
                        _zobristUpdater.AddEnPassant(ref _hash, Color.Black, enPassantLSB);
                    }
                }
            }
        }

        void CalculateOccupancy()
        {
            for(int piece=0; piece<6; piece++)
            {
                _occupancy[(int)Color.White] |= _pieces[FastArray.GetPieceIndex(Color.White, (PieceType)piece)];
                _occupancy[(int)Color.Black] |= _pieces[FastArray.GetPieceIndex(Color.Black, (PieceType)piece)];
            }
        }

        void CalculateAvailableMoves(GeneratorMode whiteMode, GeneratorMode blackMode)
        {
            var whiteGeneratorParameters = GetGeneratorParameters(Color.White, whiteMode);
            var blackGeneratorParameters = GetGeneratorParameters(Color.Black, blackMode);
            
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
            {
                return;
            }

            _kingMovesGenerator.CalculateCastling(generatorParameters);
        }

        GeneratorParameters GetGeneratorParameters(Color color, GeneratorMode mode)
        {
            return new GeneratorParameters()
            {
                FriendlyColor = color,
                EnemyColor = ColorOperations.Invert(color),
                Mode = mode,

                Pieces = _pieces,
                EnPassant = _enPassant,
                CastlingPossibility = _castlingPossibility,

                Attacks = _attacks,
                AttacksSummary = _attacksSummary,

                Occupancy = _occupancy[(int)Color.White] | _occupancy[(int)Color.Black],
                FriendlyOccupancy = _occupancy[(int)color],
                EnemyOccupancy = _occupancy[(int)ColorOperations.Invert(color)],

                Moves = _moves
            };
        }

        EvaluationParameters GetEvaluationParameters()
        {
            return new EvaluationParameters()
            {
                GamePhase = GamePhase.Regular,

                Pieces = _pieces,
                Occupancy = _occupancy,
                EnPassant = _enPassant,

                CastlingDone = _castlingDone,

                Attacks = _attacks,
                AttacksSummary = _attacksSummary
            };
        }
    }
}
