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
        public ulong Hash { get; private set; }
        public LinkedList<Move> Moves { get; private set; }

        ulong[] _pieces;
        ulong[] _occupancy;
        ulong[] _enPassant;

        ulong[] _attacks;
        ulong[] _attacksSummary;

        bool[] _castlingPossibility;
        bool[] _castlingDone;

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

            Moves = new LinkedList<Move>();

            _zobristUpdater = new ZobristUpdater();
            _knightMovesGenerator = new KnightMovesGenerator();
            _kingMovesGenerator = new KingMovesGenerator();
            _rookMovesGenerator = new RookMovesGenerator();
            _bishopMovesGenerator = new BishopMovesGenerator();
            _pawnMovesGenerator = new PawnMovesGenerator();
        }

        public BitBoard(BitBoard bitBoard, Move move) : this()
        {
            Hash = bitBoard.Hash;
            Hash = _zobristUpdater.ClearEnPassant(Hash, ColorOperations.Invert(move.Color), bitBoard._enPassant);

            Buffer.BlockCopy(bitBoard._pieces, 0, _pieces, 0, bitBoard._pieces.Length * sizeof(ulong));
            Buffer.BlockCopy(bitBoard._castlingPossibility, 0, _castlingPossibility, 0, bitBoard._castlingPossibility.Length * sizeof(bool));
            Buffer.BlockCopy(bitBoard._castlingDone, 0, _castlingDone, 0, bitBoard._castlingDone.Length * sizeof(bool));
            Buffer.BlockCopy(bitBoard._occupancy, 0, _occupancy, 0, bitBoard._occupancy.Length * sizeof(ulong));

            CalculateMove(bitBoard, move);
            CalculateEnPassant(move);
        }

        public BitBoard(FriendlyBoard friendlyBoard) : this()
        {
            _pieces = friendlyBoard.GetPiecesArray();
            _castlingPossibility = friendlyBoard.GetCastlingPossibilityArray();
            _castlingDone = friendlyBoard.GetCastlingDoneArray();
            _enPassant = friendlyBoard.GetEnPassantArray();
            _occupancy = CalculateOccupancy();

            Hash = GetNewHash();
        }

        public BitBoard Move(Move move)
        {
            return new BitBoard(this, move);
        }
        
        public FriendlyBoard GetFriendlyBoard()
        {
            return new FriendlyBoard(_pieces, _attacks, _castlingPossibility, _castlingDone, _enPassant);
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
            CalculateAvailableMoves(whiteMode, blackMode);
        }

        public EvaluationResult GetEvaluation()
        {
            var evaluationCalculator = new EvaluationCalculator();
            var evaluationParameters = GetEvaluationParameters();

            return evaluationCalculator.GetEvaluation(evaluationParameters);
        }

        public bool VerifyIntegrity()
        {
            var calculatedOccupancy = CalculateOccupancy();

            return Hash == GetNewHash() &&
                   calculatedOccupancy[(int)Color.White] == _occupancy[(int)Color.White] &&
                   calculatedOccupancy[(int)Color.Black] == _occupancy[(int)Color.Black];
        }

        ulong GetNewHash()
        {
            return new ZobristHash().Calculate(_pieces, _castlingPossibility, _enPassant);
        }

        void CalculateMove(BitBoard bitBoard, Move move)
        {
            switch(move)
            {
                case QuietMove quietMove:           { CalculateQuietMove(quietMove); break; }
                case KillMove killMove:             { CalculateKillMove(killMove); break; }
                case EnPassantMove enPassantMove:   { CalculateEnPassantMove(enPassantMove); break; }
                case CastlingMove castlingMove:     { CalculateCastlingMove(castlingMove); break; }
                case PromotionMove promotionMove:   { CalculatePromotionMove(promotionMove); break; }
            }
        }

        void CalculateQuietMove(QuietMove move)
        {
            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);
            var change = from | to;

            if (move.Piece == PieceType.King)
            {
                var shortCastlingIndex = FastArray.GetCastlingIndex(move.Color, CastlingType.Short);
                var longCastlingIndex = FastArray.GetCastlingIndex(move.Color, CastlingType.Long);

                Hash = _zobristUpdater.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Short);
                Hash = _zobristUpdater.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Long);

                _castlingPossibility[shortCastlingIndex] = false;
                _castlingPossibility[longCastlingIndex] = false;
            }
            else if (move.Piece == PieceType.Rook)
            {
                if (move.From == new Position(1, 1) || move.From == new Position(1, 8))
                {
                    Hash = _zobristUpdater.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Long);
                    _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;
                }
                else if (move.From == new Position(8, 1) || move.From == new Position(8, 8))
                {
                    Hash = _zobristUpdater.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Short);
                    _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
                }
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] ^= change;
            _occupancy[(int)move.Color] ^= change;

            Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, move.Piece, from);
            Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, move.Piece, to);
        }

        void CalculateKillMove(KillMove move)
        {
            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);
            var change = from | to;

            var enemyColor = ColorOperations.Invert(move.Color);

            for (int piece = 0; piece < 6; piece++)
            {
                var index = FastArray.GetPieceIndex(enemyColor, (PieceType)piece);
                if((_pieces[index] & to) != 0)
                {
                    _pieces[index] &= ~to;
                    _occupancy[(int)enemyColor] ^= to;

                    Hash = _zobristUpdater.AddOrRemovePiece(Hash, enemyColor, (PieceType)piece, to);
                    break;
                }
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] ^= change;
            _occupancy[(int)move.Color] ^= change;

            Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, move.Piece, from);
            Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, move.Piece, to);
        }

        void CalculateCastlingMove(CastlingMove move)
        {
            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);
            var change = from | to;

            switch (move.CastlingType)
            {
                case CastlingType.Short:
                {
                    var rookLSB = move.Color == Color.White ? KingMovesGenerator.WhiteRightRookLSB : KingMovesGenerator.BlackRightRookLSB;
                    var rookChange = rookLSB | (rookLSB << 2);

                    _pieces[FastArray.GetPieceIndex(move.Color, PieceType.Rook)] ^= rookChange;
                    _occupancy[(int)move.Color] ^= rookChange;

                    Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, PieceType.Rook, rookLSB);
                    Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, PieceType.Rook, rookLSB << 2);

                    break;
                }
                case CastlingType.Long:
                {
                    var rookLSB = move.Color == Color.White ? KingMovesGenerator.WhiteLeftRookLSB : KingMovesGenerator.BlackLeftRookLSB;
                    var rookChange = rookLSB | (rookLSB >> 3);

                    _pieces[FastArray.GetPieceIndex(move.Color, PieceType.Rook)] ^= rookChange;
                    _occupancy[(int)move.Color] ^= rookChange;

                    Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, PieceType.Rook, rookLSB);
                    Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, PieceType.Rook, rookLSB >> 3);

                    break;
                }
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] ^= change;
            _occupancy[(int)move.Color] ^= change;

            Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, move.Piece, from);
            Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, move.Piece, to);
            Hash = _zobristUpdater.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Short);
            Hash = _zobristUpdater.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Long);

            _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
            _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;

            _castlingDone[(int)move.Color] = true;
        }

        void CalculateEnPassantMove(EnPassantMove move)
        {
            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);
            var change = from | to;

            var enemyColor = ColorOperations.Invert(move.Color);

            if (move.Color == Color.White)
            {
                _pieces[FastArray.GetPieceIndex(enemyColor, move.Piece)] &= ~(to >> 8);
                _occupancy[(int)enemyColor] ^= to >> 8;

                Hash = _zobristUpdater.AddOrRemovePiece(Hash, enemyColor, move.Piece, to >> 8);
            }
            else
            {
                _pieces[FastArray.GetPieceIndex(enemyColor, move.Piece)] &= ~(to << 8);
                _occupancy[(int)enemyColor] ^= to << 8;

                Hash = _zobristUpdater.AddOrRemovePiece(Hash, enemyColor, move.Piece, to << 8);
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] ^= change;
            _occupancy[(int)move.Color] ^= change;

            Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, move.Piece, from);
            Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, move.Piece, to);
        }

        void CalculatePromotionMove(PromotionMove move)
        {
            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);
            var change = from | to;

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] &= ~from;
            _pieces[FastArray.GetPieceIndex(move.Color, move.PromotionPiece)] |= to;
            _occupancy[(int)move.Color] ^= change;

            Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, move.Piece, from);
            Hash = _zobristUpdater.AddOrRemovePiece(Hash, move.Color, move.PromotionPiece, to);
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
                        Hash = _zobristUpdater.AddEnPassant(Hash, Color.White, enPassantLSB);
                    }
                }
                else
                {
                    if (move.From.Y == 7 && move.To.Y == 5)
                    {
                        var enPassantPosition = new Position(move.To.X, move.To.Y + 1);
                        var enPassantLSB = BitPositionConverter.ToULong(enPassantPosition);

                        _enPassant[(int)Color.Black] |= enPassantLSB;
                        Hash = _zobristUpdater.AddEnPassant(Hash, Color.Black, enPassantLSB);
                    }
                }
            }
        }

        ulong[] CalculateOccupancy()
        {
            var occupancy = new ulong[2];

            for(int piece=0; piece<6; piece++)
            {
                occupancy[(int)Color.White] |= _pieces[FastArray.GetPieceIndex(Color.White, (PieceType)piece)];
                occupancy[(int)Color.Black] |= _pieces[FastArray.GetPieceIndex(Color.Black, (PieceType)piece)];
            }

            return occupancy;
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

                Moves = Moves
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
