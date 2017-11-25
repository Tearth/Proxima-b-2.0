using System;
using System.Collections.Generic;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Evaluation;
using Proxima.Core.Evaluation.Castling;
using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Position;
using Proxima.Core.MoveGenerators;

namespace Proxima.Core.Boards
{
    public class BitBoard
    {
        public LinkedList<Move> Moves { get; private set; }
        public ulong Hash { get; private set; }

        private ulong[] _pieces;
        private ulong[] _occupancy;
        private ulong[] _enPassant;

        private ulong[] _attacks;
        private ulong[] _attacksSummary;

        private bool[] _castlingPossibility;
        private bool[] _castlingDone;

        private IncrementalEvaluationData _incrementalEvaluation;

        public BitBoard()
        {
            Moves = new LinkedList<Move>();

            _pieces = new ulong[12];
            _occupancy = new ulong[2];
            _enPassant = new ulong[2];

            _attacks = new ulong[64];
            _attacksSummary = new ulong[2];

            _castlingPossibility = new bool[4];
            _castlingDone = new bool[2];
        }

        public BitBoard(BitBoard bitBoard, Move move) : this()
        {
            Hash = bitBoard.Hash;
            Hash = IncrementalZobrist.ClearEnPassant(Hash, ColorOperations.Invert(move.Color), bitBoard._enPassant);

            Buffer.BlockCopy(bitBoard._pieces, 0, _pieces, 0, bitBoard._pieces.Length * sizeof(ulong));
            Buffer.BlockCopy(bitBoard._castlingPossibility, 0, _castlingPossibility, 0, bitBoard._castlingPossibility.Length * sizeof(bool));
            Buffer.BlockCopy(bitBoard._castlingDone, 0, _castlingDone, 0, bitBoard._castlingDone.Length * sizeof(bool));
            Buffer.BlockCopy(bitBoard._occupancy, 0, _occupancy, 0, bitBoard._occupancy.Length * sizeof(ulong));

            _incrementalEvaluation = bitBoard._incrementalEvaluation;

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

            _incrementalEvaluation = new IncrementalEvaluationData(GetDetailedEvaluation());

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

        public int GetEvaluation()
        {
            var evaluationParameters = GetEvaluationParameters();
            return EvaluationCalculator.GetEvaluation(evaluationParameters, _incrementalEvaluation);
        }

        public DetailedEvaluationData GetDetailedEvaluation()
        {
            var evaluationParameters = GetEvaluationParameters();
            return EvaluationCalculator.GetDetailedEvaluation(evaluationParameters);
        }

        public bool VerifyIntegrity()
        {
            var calculatedOccupancy = CalculateOccupancy();
            var calculatedEvaluation = GetDetailedEvaluation();

            return Hash == GetNewHash() &&
                   _occupancy[(int)Color.White] == calculatedOccupancy[(int)Color.White] &&
                   _occupancy[(int)Color.Black] == calculatedOccupancy[(int)Color.Black] &&
                   _incrementalEvaluation.Material == calculatedEvaluation.Material.Difference &&
                   _incrementalEvaluation.Position == calculatedEvaluation.Position.Difference &&
                   _incrementalEvaluation.Castling == calculatedEvaluation.Castling.Difference;
        }

        private ulong GetNewHash()
        {
            return ZobristHash.Calculate(_pieces, _castlingPossibility, _enPassant);
        }
        
        private void CalculateMove(BitBoard bitBoard, Move move)
        {
            switch (move)
            {
                case QuietMove quietMove: { CalculateQuietMove(quietMove); break; }
                case KillMove killMove: { CalculateKillMove(killMove); break; }
                case EnPassantMove enPassantMove: { CalculateEnPassantMove(enPassantMove); break; }
                case CastlingMove castlingMove: { CalculateCastlingMove(castlingMove); break; }
                case PromotionMove promotionMove: { CalculatePromotionMove(promotionMove); break; }
            }
        }

        private void CalculateQuietMove(QuietMove move)
        {
            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);
            var change = from | to;

            if (move.Piece == PieceType.King)
            {
                var shortCastlingIndex = FastArray.GetCastlingIndex(move.Color, CastlingType.Short);
                var longCastlingIndex = FastArray.GetCastlingIndex(move.Color, CastlingType.Long);

                Hash = IncrementalZobrist.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Short);
                Hash = IncrementalZobrist.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Long);

                _castlingPossibility[shortCastlingIndex] = false;
                _castlingPossibility[longCastlingIndex] = false;
            }
            else if (move.Piece == PieceType.Rook)
            {
                if (move.From == new Position(1, 1) || move.From == new Position(1, 8))
                {
                    Hash = IncrementalZobrist.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Long);
                    _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;
                }
                else if (move.From == new Position(8, 1) || move.From == new Position(8, 8))
                {
                    Hash = IncrementalZobrist.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Short);
                    _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
                }
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] ^= change;
            _occupancy[(int)move.Color] ^= change;

            _incrementalEvaluation.Position = IncrementalPosition.RemovePiece(_incrementalEvaluation.Position, move.Color, move.Piece, from, GamePhase.Regular);
            _incrementalEvaluation.Position = IncrementalPosition.AddPiece(_incrementalEvaluation.Position, move.Color, move.Piece, to, GamePhase.Regular);

            Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, move.Piece, from);
            Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, move.Piece, to);
        }

        private void CalculateKillMove(KillMove move)
        {
            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);
            var change = from | to;

            var enemyColor = ColorOperations.Invert(move.Color);

            for (int piece = 0; piece < 6; piece++)
            {
                var index = FastArray.GetPieceIndex(enemyColor, (PieceType)piece);
                if ((_pieces[index] & to) != 0)
                {
                    _pieces[index] &= ~to;
                    _occupancy[(int)enemyColor] ^= to;

                    _incrementalEvaluation.Material = IncrementalMaterial.RemovePiece(_incrementalEvaluation.Material, (PieceType)piece, enemyColor);
                    _incrementalEvaluation.Position = IncrementalPosition.RemovePiece(_incrementalEvaluation.Position, enemyColor, (PieceType)piece, to, GamePhase.Regular);
                    Hash = IncrementalZobrist.AddOrRemovePiece(Hash, enemyColor, (PieceType)piece, to);
                    break;
                }
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] ^= change;
            _occupancy[(int)move.Color] ^= change;

            _incrementalEvaluation.Position = IncrementalPosition.RemovePiece(_incrementalEvaluation.Position, move.Color, move.Piece, from, GamePhase.Regular);
            _incrementalEvaluation.Position = IncrementalPosition.AddPiece(_incrementalEvaluation.Position, move.Color, move.Piece, to, GamePhase.Regular);

            Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, move.Piece, from);
            Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, move.Piece, to);
        }

        private void CalculateCastlingMove(CastlingMove move)
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

                    _incrementalEvaluation.Position = IncrementalPosition.RemovePiece(_incrementalEvaluation.Position, move.Color, PieceType.Rook, rookLSB, GamePhase.Regular);
                    _incrementalEvaluation.Position = IncrementalPosition.AddPiece(_incrementalEvaluation.Position, move.Color, PieceType.Rook, rookLSB << 2, GamePhase.Regular);

                    Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, PieceType.Rook, rookLSB);
                    Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, PieceType.Rook, rookLSB << 2);

                    break;
                }

                case CastlingType.Long:
                {
                    var rookLSB = move.Color == Color.White ? KingMovesGenerator.WhiteLeftRookLSB : KingMovesGenerator.BlackLeftRookLSB;
                    var rookChange = rookLSB | (rookLSB >> 3);

                    _pieces[FastArray.GetPieceIndex(move.Color, PieceType.Rook)] ^= rookChange;
                    _occupancy[(int)move.Color] ^= rookChange;

                    _incrementalEvaluation.Position = IncrementalPosition.RemovePiece(_incrementalEvaluation.Position, move.Color, PieceType.Rook, rookLSB, GamePhase.Regular);
                    _incrementalEvaluation.Position = IncrementalPosition.AddPiece(_incrementalEvaluation.Position, move.Color, PieceType.Rook, rookLSB >> 3, GamePhase.Regular);

                    Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, PieceType.Rook, rookLSB);
                    Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, PieceType.Rook, rookLSB >> 3);

                    break;
                }
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] ^= change;
            _occupancy[(int)move.Color] ^= change;

            Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, move.Piece, from);
            Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, move.Piece, to);
            Hash = IncrementalZobrist.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Short);
            Hash = IncrementalZobrist.RemoveCastlingPossibility(Hash, _castlingPossibility, move.Color, CastlingType.Long);

            _incrementalEvaluation.Position = IncrementalPosition.RemovePiece(_incrementalEvaluation.Position, move.Color, move.Piece, from, GamePhase.Regular);
            _incrementalEvaluation.Position = IncrementalPosition.AddPiece(_incrementalEvaluation.Position, move.Color, move.Piece, to, GamePhase.Regular);

            _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Short)] = false;
            _castlingPossibility[FastArray.GetCastlingIndex(move.Color, CastlingType.Long)] = false;

            _incrementalEvaluation.Castling = IncrementalCastling.SetCastlingDone(_incrementalEvaluation.Castling, move.Color, GamePhase.Regular);
            _castlingDone[(int)move.Color] = true;
        }

        private void CalculateEnPassantMove(EnPassantMove move)
        {
            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);
            var change = from | to;

            var enemyColor = ColorOperations.Invert(move.Color);

            if (move.Color == Color.White)
            {
                _pieces[FastArray.GetPieceIndex(enemyColor, PieceType.Pawn)] &= ~(to >> 8);
                _occupancy[(int)enemyColor] ^= to >> 8;

                _incrementalEvaluation.Material = IncrementalMaterial.RemovePiece(_incrementalEvaluation.Material, PieceType.Pawn, enemyColor);
                _incrementalEvaluation.Position = IncrementalPosition.RemovePiece(_incrementalEvaluation.Position, enemyColor, PieceType.Pawn, to >> 8, GamePhase.Regular);
                Hash = IncrementalZobrist.AddOrRemovePiece(Hash, enemyColor, PieceType.Pawn, to >> 8);
            }
            else
            {
                _pieces[FastArray.GetPieceIndex(enemyColor, PieceType.Pawn)] &= ~(to << 8);
                _occupancy[(int)enemyColor] ^= to << 8;

                _incrementalEvaluation.Material = IncrementalMaterial.RemovePiece(_incrementalEvaluation.Material, PieceType.Pawn, enemyColor);
                _incrementalEvaluation.Position = IncrementalPosition.RemovePiece(_incrementalEvaluation.Position, enemyColor, PieceType.Pawn, to << 8, GamePhase.Regular);
                Hash = IncrementalZobrist.AddOrRemovePiece(Hash, enemyColor, PieceType.Pawn, to << 8);
            }

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] ^= change;
            _occupancy[(int)move.Color] ^= change;

            _incrementalEvaluation.Position = IncrementalPosition.RemovePiece(_incrementalEvaluation.Position, move.Color, move.Piece, from, GamePhase.Regular);
            _incrementalEvaluation.Position = IncrementalPosition.AddPiece(_incrementalEvaluation.Position, move.Color, move.Piece, to, GamePhase.Regular);

            Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, move.Piece, from);
            Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, move.Piece, to);
        }

        private void CalculatePromotionMove(PromotionMove move)
        {
            var from = BitPositionConverter.ToULong(move.From);
            var to = BitPositionConverter.ToULong(move.To);
            var change = from | to;

            _pieces[FastArray.GetPieceIndex(move.Color, move.Piece)] &= ~from;
            _pieces[FastArray.GetPieceIndex(move.Color, move.PromotionPiece)] |= to;
            _occupancy[(int)move.Color] ^= change;

            _incrementalEvaluation.Material = IncrementalMaterial.RemovePiece(_incrementalEvaluation.Material, move.Piece, move.Color);
            _incrementalEvaluation.Material = IncrementalMaterial.AddPiece(_incrementalEvaluation.Material, move.PromotionPiece, move.Color);

            _incrementalEvaluation.Position = IncrementalPosition.RemovePiece(_incrementalEvaluation.Position, move.Color, move.Piece, from, GamePhase.Regular);
            _incrementalEvaluation.Position = IncrementalPosition.AddPiece(_incrementalEvaluation.Position, move.Color, move.PromotionPiece, to, GamePhase.Regular);

            Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, move.Piece, from);
            Hash = IncrementalZobrist.AddOrRemovePiece(Hash, move.Color, move.PromotionPiece, to);
        }

        private void CalculateEnPassant(Move move)
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
                        Hash = IncrementalZobrist.AddEnPassant(Hash, Color.White, enPassantLSB);
                    }
                }
                else
                {
                    if (move.From.Y == 7 && move.To.Y == 5)
                    {
                        var enPassantPosition = new Position(move.To.X, move.To.Y + 1);
                        var enPassantLSB = BitPositionConverter.ToULong(enPassantPosition);

                        _enPassant[(int)Color.Black] |= enPassantLSB;
                        Hash = IncrementalZobrist.AddEnPassant(Hash, Color.Black, enPassantLSB);
                    }
                }
            }
        }

        private ulong[] CalculateOccupancy()
        {
            var occupancy = new ulong[2];

            for (int piece = 0; piece < 6; piece++)
            {
                occupancy[(int)Color.White] |= _pieces[FastArray.GetPieceIndex(Color.White, (PieceType)piece)];
                occupancy[(int)Color.Black] |= _pieces[FastArray.GetPieceIndex(Color.Black, (PieceType)piece)];
            }

            return occupancy;
        }

        private void CalculateAvailableMoves(GeneratorMode whiteMode, GeneratorMode blackMode)
        {
            var whiteGeneratorParameters = GetGeneratorParameters(Color.White, whiteMode);
            var blackGeneratorParameters = GetGeneratorParameters(Color.Black, blackMode);

            CalculateAvailableMoves(whiteGeneratorParameters);
            CalculateAvailableMoves(blackGeneratorParameters);

            CalculateCastling(whiteGeneratorParameters);
            CalculateCastling(blackGeneratorParameters);
        }

        private void CalculateAvailableMoves(GeneratorParameters generatorParameters)
        {
            PawnMovesGenerator.Calculate(generatorParameters);
            KnightMovesGenerator.Calculate(generatorParameters);
            KingMovesGenerator.Calculate(generatorParameters);

            RookMovesGenerator.Calculate(PieceType.Rook, generatorParameters);
            BishopMovesGenerator.Calculate(PieceType.Bishop, generatorParameters);

            RookMovesGenerator.Calculate(PieceType.Queen, generatorParameters);
            BishopMovesGenerator.Calculate(PieceType.Queen, generatorParameters);
        }

        private void CalculateCastling(GeneratorParameters generatorParameters)
        {
            if ((generatorParameters.Mode & GeneratorMode.CalculateMoves) == 0)
            {
                return;
            }

            KingMovesGenerator.CalculateCastling(generatorParameters);
        }

        private GeneratorParameters GetGeneratorParameters(Color color, GeneratorMode mode)
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

        private EvaluationParameters GetEvaluationParameters()
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
