using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.MagicBitboards;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.MoveGenerators
{
    /// <summary>
    /// Represents a set of methods to generating rook moves.
    /// </summary>
    public static class RookMovesGenerator
    {
        /// <summary>
        /// Generates available moves.
        /// </summary>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="opt">The generator parameters.</param>
        public static void Generate(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(piecesToParse);
                piecesToParse = BitOperations.PopLSB(piecesToParse);

                var excludeFromAttacks = CalculateMoves(pieceType, pieceLSB, opt);
                CalculateAttacks(pieceLSB, excludeFromAttacks, opt);
            }
        }

        /// <summary>
        /// Calculates moves for the specified piece.
        /// </summary>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="pieceBitboard">The bitboard with set bit at piece position.</param>
        /// <param name="opt">The generator parameters.</param>
        /// <returns>The bitboard with available moves for the specified piece.</returns>
        private static ulong CalculateMoves(PieceType pieceType, ulong pieceBitboard, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
            {
                return 0;
            }

            var pieceIndex = BitOperations.GetBitIndex(pieceBitboard);
            var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

            var pattern = MagicContainer.GetRookAttacks(pieceIndex, opt.OccupancySummary);
            pattern &= ~opt.FriendlyOccupancy;

            if (opt.QuiescenceSearch)
            {
                pattern &= opt.EnemyOccupancy;
            }

            var excludeFromAttacks = pattern;
            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);
                    
                var to = BitPositionConverter.ToPosition(patternIndex);

                if ((patternLSB & opt.EnemyOccupancy) == 0)
                {
                    opt.Bitboard.Moves.AddLast(new QuietMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }
                else
                {
                    opt.Bitboard.Moves.AddLast(new KillMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }

                if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                {
                    opt.Bitboard.Attacks[patternIndex] |= pieceBitboard;
                    opt.Bitboard.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                }
            }

            return excludeFromAttacks;
        }

        /// <summary>
        /// Calculates attacks for the specified piece.
        /// </summary>
        /// <param name="pieceBitboard">The bitboard with set bit at piece position.</param>
        /// <param name="excludedFields">The bitboard with excluded fields from attacks calculating.</param>
        /// <param name="opt">The generator parameters.</param>
        private static void CalculateAttacks(ulong pieceBitboard, ulong excludedFields, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateAttacks) == 0)
            {
                return;
            }

            var blockersToRemove = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Rook)] |
                                   opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Queen)];

            var occupancyWithoutBlockers = opt.OccupancySummary & ~blockersToRemove;          
            var pieceIndex = BitOperations.GetBitIndex(pieceBitboard);

            var pattern = MagicContainer.GetRookAttacks(pieceIndex, occupancyWithoutBlockers);
            pattern ^= excludedFields;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                opt.Bitboard.Attacks[patternIndex] |= pieceBitboard;
                opt.Bitboard.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
            }
        }
    }
}
