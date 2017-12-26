using System;
using System.Collections.Generic;
using Proxima.Core.AI;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Evaluation;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.Boards
{
    /// <summary>
    /// The soul of the chess engine. Contains all information about single board state (pieces, castling flags,
    /// en passant etc.). Because this representation is not friendly to displaying, it can be easily converted
    /// to <see cref="FriendlyEnPassant"/> object.
    /// </summary>
    public class Bitboard
    {
        /// <summary>
        /// Gets or sets the board hash (calculated by <see cref="ZobristHash"/> or <see cref="IncrementalZobrist"/>.
        /// </summary>
        public ulong Hash { get; set; }

        /// <summary>
        /// Gets or sets the game phase.
        /// </summary>
        public GamePhase GamePhase { get; set; }

        /// <summary>
        /// Gets the pieces array (more information about piece indexes at <see cref="PieceType"/>.
        /// </summary>
        public ulong[] Pieces { get; private set; }

        /// <summary>
        /// Gets the occupancy array (0 = White, 1 = Black).
        /// </summary>
        public ulong[] Occupancy { get; private set; }

        /// <summary>
        /// Gets the en passant array (0 = White, 1 = Black).
        /// </summary>
        public ulong[] EnPassant { get; private set; }

        /// <summary>
        /// Gets the attacks array (access by field indexes).
        /// </summary>
        public ulong[] Attacks { get; private set; }

        /// <summary>
        /// Gets the attacks summary array (0 = White, 1 = Black).
        /// </summary>
        public ulong[] AttacksSummary { get; private set; }

        /// <summary>
        /// Gets the castling possibility array (more information about castling indexes at <see cref="CastlingType"/>).
        /// </summary>
        public bool[] CastlingPossibility { get; private set; }

        /// <summary>
        /// Gets the castling done array (0 = White, 1 = Black).
        /// </summary>
        public bool[] CastlingDone { get; private set; }

        /// <summary>
        /// Gets the available moves list (only if <see cref="Calculate(bool)"/> method was called).
        /// </summary>
        public LinkedList<Move> Moves { get; private set; }

        /// <summary>
        /// Gets the incremental evaluation data.
        /// </summary>
        public IncrementalEvaluationData IncEvaluation { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitboard"/> class.
        /// </summary>
        public Bitboard()
        {
            Pieces = new ulong[12];
            Occupancy = new ulong[2];
            EnPassant = new ulong[2];

            Attacks = new ulong[64];
            AttacksSummary = new ulong[2];

            CastlingPossibility = new bool[4];
            CastlingDone = new bool[2];

            Moves = new LinkedList<Move>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitboard"/> class.
        /// </summary>
        /// <param name="bitboard">The previous bitboard to copy.</param>
        public Bitboard(Bitboard bitboard) : this()
        {
            Hash = bitboard.Hash;

            Buffer.BlockCopy(bitboard.Pieces, 0, Pieces, 0, bitboard.Pieces.Length * sizeof(ulong));
            Buffer.BlockCopy(bitboard.CastlingPossibility, 0, CastlingPossibility, 0, bitboard.CastlingPossibility.Length * sizeof(bool));
            Buffer.BlockCopy(bitboard.CastlingDone, 0, CastlingDone, 0, bitboard.CastlingDone.Length * sizeof(bool));
            Buffer.BlockCopy(bitboard.Occupancy, 0, Occupancy, 0, bitboard.Occupancy.Length * sizeof(ulong));
            Buffer.BlockCopy(bitboard.EnPassant, 0, EnPassant, 0, bitboard.EnPassant.Length * sizeof(ulong));

            IncEvaluation = new IncrementalEvaluationData(bitboard.IncEvaluation);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitboard"/> class.
        /// </summary>
        /// <param name="bitboard">The previous bitboard.</param>
        /// <param name="move">The move to apply.</param>
        public Bitboard(Bitboard bitboard, Move move) : this(bitboard)
        {
            IncrementalZobrist.ClearEnPassant(ColorOperations.Invert(move.Color), this);

            EnPassant[(int)ColorOperations.Invert(move.Color)] = 0;
            move.Do(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitboard"/> class.
        /// </summary>
        /// <param name="friendlyBoard">The friendly board.</param>
        public Bitboard(FriendlyBoard friendlyBoard) : this()
        {
            Pieces = friendlyBoard.GetPiecesArray();
            CastlingPossibility = friendlyBoard.GetCastlingPossibilityArray();
            CastlingDone = friendlyBoard.GetCastlingDoneArray();
            EnPassant = friendlyBoard.GetEnPassantArray();
            Occupancy = CalculateOccupancy();

            IncEvaluation = new IncrementalEvaluationData(GetDetailedEvaluation());

            Hash = GetNewHash();
        }

        /// <summary>
        /// Does the specified move at cloned bitboard.
        /// </summary>
        /// <param name="move">The move to apply.</param>
        /// <returns>The bitboard with applied move.</returns>
        public Bitboard Move(Move move)
        {
            return new Bitboard(this, move);
        }

        /// <summary>
        /// Checks if king with the specified color is checked.
        /// </summary>
        /// <param name="color">The king color.</param>
        /// <returns>True if king with specified color is checked, otherwise false.</returns>
        public bool IsCheck(Color color)
        {
            var enemyColor = ColorOperations.Invert(color);
            var king = Pieces[FastArray.GetPieceIndex(color, PieceType.King)];

            return (AttacksSummary[(int)enemyColor] & king) != 0;
        }

        /// <summary>
        /// Checks if king with the specified color is mated.
        /// </summary>
        /// <param name="color">The king color.</param>
        /// <returns>True if king with specified color is mated, otherwise false.</returns>
        public bool IsMate(Color color)
        {
            var ai = new AICore();
            var aiResult = ai.Calculate(color, this, 0);

            return IsCheck(color) && Math.Abs(aiResult.Score) == AIConstants.MateValue;
        }

        /// <summary>
        /// Checks if king with the specified color is in stalemate.
        /// </summary>
        /// <param name="color">The king color.</param>
        /// <returns>True if king with specified color is in stalemate, otherwise false.</returns>
        public bool IsStalemate(Color color)
        {
            var ai = new AICore();
            var aiResult = ai.Calculate(color, this, 0);

            return !IsCheck(color) && Math.Abs(aiResult.Score) == AIConstants.MateValue;
        }

        /// <summary>
        /// Calculates available moves.
        /// </summary>
        /// <param name="quiescenceSearch">If true, only quiescence moves (mainly captures) will be generated.</param>
        public void Calculate(bool quiescenceSearch)
        {
            var generatorMode = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            CalculateAvailableMoves(generatorMode, generatorMode, quiescenceSearch);
        }

        /// <summary>
        /// Calculates available moves.
        /// </summary>
        /// <param name="mode">The generator mode for both colors.</param>
        /// <param name="quiescenceSearch">If true, only quiescence moves (mainly captures) will be generated.</param>
        public void Calculate(GeneratorMode mode, bool quiescenceSearch)
        {
            CalculateAvailableMoves(mode, mode, quiescenceSearch);
        }

        /// <summary>
        /// Calculates available moves.
        /// </summary>
        /// <param name="whiteMode">The white generator mode.</param>
        /// <param name="blackMode">The black generator mode.</param>
        /// <param name="quiescenceSearch">If true, only quiescence moves (mainly captures) will be generated.</param>
        public void Calculate(GeneratorMode whiteMode, GeneratorMode blackMode, bool quiescenceSearch)
        {
            CalculateAvailableMoves(whiteMode, blackMode, quiescenceSearch);
        }

        /// <summary>
        /// Calculates board evaluation.
        /// </summary>
        /// <returns>The board evaluation data.</returns>
        public int GetEvaluation()
        {
            return EvaluationCalculator.GetEvaluation(this);
        }

        /// <summary>
        /// Calculates detailed (separated into individual components and colors) board evaluation.
        /// </summary>
        /// <returns>The board evaluation data.</returns>
        public DetailedEvaluationData GetDetailedEvaluation()
        {
            return EvaluationCalculator.GetDetailedEvaluation(this);
        }

        /// <summary>
        /// Verifies board integrity (compares incremental occupancy and evaluation with fresh calculations).
        /// </summary>
        /// <returns>True if board is integral, otherwise false.</returns>
        public bool VerifyIntegrity()
        {
            var calculatedOccupancy = CalculateOccupancy();
            var calculatedEvaluation = GetDetailedEvaluation();

            return Hash == GetNewHash() &&
                   Occupancy[(int)Color.White] == calculatedOccupancy[(int)Color.White] &&
                   Occupancy[(int)Color.Black] == calculatedOccupancy[(int)Color.Black] &&
                   IncEvaluation.Material == calculatedEvaluation.Material.Difference &&
                   IncEvaluation.Position == calculatedEvaluation.Position.Difference &&
                   IncEvaluation.Castling == calculatedEvaluation.Castling.Difference;
        }

        /// <summary>
        /// Calculates Zobrist hash.
        /// </summary>
        /// <returns>The Zobrst hash for the current bitboard.</returns>
        private ulong GetNewHash()
        {
            return ZobristHash.Calculate(Pieces, CastlingPossibility, EnPassant);
        }

        /// <summary>
        /// Calculates occupancy for all players.
        /// </summary>
        /// <returns>The occupancy array.</returns>
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

        /// <summary>
        /// Calculates available moves.
        /// </summary>
        /// <param name="whiteMode">The white generator mode.</param>
        /// <param name="blackMode">The black generator mode.</param>
        /// <param name="quiescenceSearch">If true, only quiescence moves (mainly captures) will be generated.</param>
        private void CalculateAvailableMoves(GeneratorMode whiteMode, GeneratorMode blackMode, bool quiescenceSearch)
        {
            var whiteGeneratorParameters = GetGeneratorParameters(Color.White, whiteMode, quiescenceSearch);
            var blackGeneratorParameters = GetGeneratorParameters(Color.Black, blackMode, quiescenceSearch);

            CalculateAvailableMoves(whiteGeneratorParameters);
            CalculateAvailableMoves(blackGeneratorParameters);

            CalculateCastling(whiteGeneratorParameters);
            CalculateCastling(blackGeneratorParameters);
        }

        /// <summary>
        /// Calculates available moves.
        /// </summary>
        /// <param name="generatorParameters">The generator parameters.</param>
        private void CalculateAvailableMoves(GeneratorParameters generatorParameters)
        {
            PawnMovesGenerator.Generate(generatorParameters);
            KnightMovesGenerator.Generate(generatorParameters);
            KingMovesGenerator.Generate(generatorParameters);

            RookMovesGenerator.Generate(PieceType.Rook, generatorParameters);
            BishopMovesGenerator.Generate(PieceType.Bishop, generatorParameters);

            RookMovesGenerator.Generate(PieceType.Queen, generatorParameters);
            BishopMovesGenerator.Generate(PieceType.Queen, generatorParameters);
        }

        /// <summary>
        /// Calculates castling moves.
        /// </summary>
        /// <param name="generatorParameters">The generator parameters.</param>
        private void CalculateCastling(GeneratorParameters generatorParameters)
        {
            if ((generatorParameters.Mode & GeneratorMode.CalculateMoves) != 0)
            {
                KingMovesGenerator.CalculateCastling(generatorParameters);
            }
        }

        /// <summary>
        /// Calculates generator parameters for the specified player and generator mode.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <param name="mode">The generator mode.</param>
        /// <param name="quiescenceSearch">If true, only quiescence moves (mainly captures) will be generated.</param>
        /// <returns>The generator parameters.</returns>
        private GeneratorParameters GetGeneratorParameters(Color color, GeneratorMode mode, bool quiescenceSearch)
        {
            return new GeneratorParameters()
            {
                Bitboard = this,

                FriendlyColor = color,
                EnemyColor = ColorOperations.Invert(color),
                Mode = mode,

                OccupancySummary = Occupancy[(int)Color.White] | Occupancy[(int)Color.Black],
                FriendlyOccupancy = Occupancy[(int)color],
                EnemyOccupancy = Occupancy[(int)ColorOperations.Invert(color)],

                QuiescenceSearch = quiescenceSearch
            };
        }
    }
}
