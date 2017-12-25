using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.MagicBitboards;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators
{
    /// <summary>
    /// Represents a set of methods to generating bishop moves.
    /// </summary>
    public static class BishopMovesGenerator
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
                CalculateAttacks(pieceType, pieceLSB, excludeFromAttacks, opt);
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

            var pattern = MagicContainer.GetBishopAttacks(pieceIndex, opt.OccupancySummary);
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
        /// <param name="pieceType">The piece type.</param>
        /// <param name="pieceBitboard">The bitboard with set bit at piece position.</param>
        /// <param name="excludedFields">The bitboard with excluded fields from attacks calculating.</param>
        /// <param name="opt">The generator parameters.</param>
        private static void CalculateAttacks(PieceType pieceType, ulong pieceBitboard, ulong excludedFields, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateAttacks) == 0)
            {
                return;
            }

            var pieceIndex = BitOperations.GetBitIndex(pieceBitboard);
            var blockersToRemove = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Bishop)] |
                                   opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Queen)];

            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];
            var allPiecesOccupancy = opt.OccupancySummary & ~blockersToRemove;

            var pattern = MagicContainer.GetBishopAttacks(pieceIndex, allPiecesOccupancy);
            pattern = CalculatePawnBlockers(pieceIndex, pattern, opt);
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

        /// <summary>
        /// Calculates X-Ray attacks when friendly pawn is on bishop way.
        /// </summary>
        /// <param name="pieceIndex">The field index with the specified bishop.</param>
        /// <param name="pattern">The bishop moves pattern.</param>
        /// <param name="opt">The generator parameters.</param>
        /// <returns>The attacks bitboard with pawn X-Ray attacks.</returns>
        private static ulong CalculatePawnBlockers(int pieceIndex, ulong pattern, GeneratorParameters opt)
        {
            var patternWithFriendlyBlockers = pattern;
            var allowedBlockers = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];

            var piecePosition = BitPositionConverter.ToPosition(pieceIndex);
            var friendlyBlockers = pattern & opt.FriendlyOccupancy & allowedBlockers;

            while (friendlyBlockers != 0)
            {
                var friendlyBlockerLSB = BitOperations.GetLSB(friendlyBlockers);
                friendlyBlockers = BitOperations.PopLSB(friendlyBlockers);

                var friendlyBlockerIndex = BitOperations.GetBitIndex(friendlyBlockerLSB);
                var friendlyBlockerPosition = BitPositionConverter.ToPosition(friendlyBlockerIndex);

                if (opt.FriendlyColor == Color.White &&
                   (friendlyBlockerPosition.X > piecePosition.X && friendlyBlockerPosition.Y > piecePosition.Y) &&
                   (friendlyBlockerLSB & (BitConstants.HFile | BitConstants.HRank)) == 0)
                {
                    patternWithFriendlyBlockers |= friendlyBlockerLSB << 7;
                }
                else if (opt.FriendlyColor == Color.White &&
                        (friendlyBlockerPosition.X < piecePosition.X && friendlyBlockerPosition.Y > piecePosition.Y) &&
                        (friendlyBlockerLSB & (BitConstants.AFile | BitConstants.HRank)) == 0)
                {
                    patternWithFriendlyBlockers |= friendlyBlockerLSB << 9;
                }
                else if (opt.FriendlyColor == Color.Black &&
                        (friendlyBlockerPosition.X > piecePosition.X && friendlyBlockerPosition.Y < piecePosition.Y) &&
                        (friendlyBlockerLSB & (BitConstants.HFile | BitConstants.ARank)) == 0)
                {
                    patternWithFriendlyBlockers |= friendlyBlockerLSB >> 9;
                }
                else if (opt.FriendlyColor == Color.Black &&
                        (friendlyBlockerPosition.X < piecePosition.X && friendlyBlockerPosition.Y < piecePosition.Y) &&
                        (friendlyBlockerLSB & (BitConstants.AFile | BitConstants.ARank)) == 0)
                {
                    patternWithFriendlyBlockers |= friendlyBlockerLSB >> 7;
                }
            }

            return patternWithFriendlyBlockers;
        }
    }
}