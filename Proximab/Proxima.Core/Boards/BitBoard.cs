using System;
using System.Collections.Generic;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Evaluation;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.Boards
{
    public class BitBoard
    {
        public LinkedList<Move> Moves { get; private set; }
        public ulong Hash { get; set; }

        public ulong[] Pieces { get; private set; }
        public ulong[] Occupancy { get; private set; }
        public ulong[] EnPassant { get; private set; }

        public ulong[] Attacks { get; private set; }
        public ulong[] AttacksSummary { get; private set; }

        public bool[] CastlingPossibility { get; private set; }
        public bool[] CastlingDone { get; private set; }

        public IncrementalEvaluationData IncrementalEvaluation { get; set; }

        public BitBoard()
        {
            Moves = new LinkedList<Move>();

            Pieces = new ulong[12];
            Occupancy = new ulong[2];
            EnPassant = new ulong[2];

            Attacks = new ulong[64];
            AttacksSummary = new ulong[2];

            CastlingPossibility = new bool[4];
            CastlingDone = new bool[2];
        }

        public BitBoard(BitBoard bitBoard, Move move) : this()
        {
            Hash = bitBoard.Hash;
            Hash = IncrementalZobrist.ClearEnPassant(Hash, ColorOperations.Invert(move.Color), bitBoard.EnPassant);

            Buffer.BlockCopy(bitBoard.Pieces, 0, Pieces, 0, bitBoard.Pieces.Length * sizeof(ulong));
            Buffer.BlockCopy(bitBoard.CastlingPossibility, 0, CastlingPossibility, 0, bitBoard.CastlingPossibility.Length * sizeof(bool));
            Buffer.BlockCopy(bitBoard.CastlingDone, 0, CastlingDone, 0, bitBoard.CastlingDone.Length * sizeof(bool));
            Buffer.BlockCopy(bitBoard.Occupancy, 0, Occupancy, 0, bitBoard.Occupancy.Length * sizeof(ulong));

            IncrementalEvaluation = (IncrementalEvaluationData)bitBoard.IncrementalEvaluation.Clone();

            move.Do(this);
            CalculateEnPassant(move);
        }

        public BitBoard(FriendlyBoard friendlyBoard) : this()
        {
            Pieces = friendlyBoard.GetPiecesArray();
            CastlingPossibility = friendlyBoard.GetCastlingPossibilityArray();
            CastlingDone = friendlyBoard.GetCastlingDoneArray();
            EnPassant = friendlyBoard.GetEnPassantArray();
            Occupancy = CalculateOccupancy();

            IncrementalEvaluation = new IncrementalEvaluationData(GetDetailedEvaluation());

            Hash = GetNewHash();
        }

        public BitBoard Move(Move move)
        {
            return new BitBoard(this, move);
        }

        public FriendlyBoard GetFriendlyBoard()
        {
            return new FriendlyBoard(Pieces, Attacks, CastlingPossibility, CastlingDone, EnPassant);
        }

        public bool IsCheck(Color color)
        {
            var enemyColor = ColorOperations.Invert(color);
            var king = Pieces[FastArray.GetPieceIndex(color, PieceType.King)];

            return (AttacksSummary[(int)enemyColor] & king) != 0;
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
            return EvaluationCalculator.GetEvaluation(this);
        }

        public DetailedEvaluationData GetDetailedEvaluation()
        {
            return EvaluationCalculator.GetDetailedEvaluation(this);
        }

        public bool VerifyIntegrity()
        {
            var calculatedOccupancy = CalculateOccupancy();
            var calculatedEvaluation = GetDetailedEvaluation();

            return Hash == GetNewHash() &&
                   Occupancy[(int)Color.White] == calculatedOccupancy[(int)Color.White] &&
                   Occupancy[(int)Color.Black] == calculatedOccupancy[(int)Color.Black] &&
                   IncrementalEvaluation.Material == calculatedEvaluation.Material.Difference &&
                   IncrementalEvaluation.Position == calculatedEvaluation.Position.Difference &&
                   IncrementalEvaluation.Castling == calculatedEvaluation.Castling.Difference;
        }

        private ulong GetNewHash()
        {
            return ZobristHash.Calculate(Pieces, CastlingPossibility, EnPassant);
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

                        EnPassant[(int)Color.White] |= enPassantLSB;
                        Hash = IncrementalZobrist.AddEnPassant(Hash, Color.White, enPassantLSB);
                    }
                }
                else
                {
                    if (move.From.Y == 7 && move.To.Y == 5)
                    {
                        var enPassantPosition = new Position(move.To.X, move.To.Y + 1);
                        var enPassantLSB = BitPositionConverter.ToULong(enPassantPosition);

                        EnPassant[(int)Color.Black] |= enPassantLSB;
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
                occupancy[(int)Color.White] |= Pieces[FastArray.GetPieceIndex(Color.White, (PieceType)piece)];
                occupancy[(int)Color.Black] |= Pieces[FastArray.GetPieceIndex(Color.Black, (PieceType)piece)];
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
                BitBoard = this,

                FriendlyColor = color,
                EnemyColor = ColorOperations.Invert(color),
                Mode = mode,

                OccupancySummary = Occupancy[(int)Color.White] | Occupancy[(int)Color.Black],
                FriendlyOccupancy = Occupancy[(int)color],
                EnemyOccupancy = Occupancy[(int)ColorOperations.Invert(color)],
            };
        }
    }
}
