using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.MagicBitboards;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators
{
    public static class BishopMovesGenerator
    {
        public static void Calculate(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.BitBoard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];
            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(piecesToParse);
                piecesToParse = BitOperations.PopLSB(piecesToParse);

                var patternContainer = CalculateMoves(pieceType, pieceLSB, opt);
                CalculateAttacks(pieceType, pieceLSB, patternContainer, opt);
            }
        }

        private static ulong CalculateMoves(PieceType pieceType, ulong pieceLSB, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
            {
                return 0;
            }

            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
            var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

            var pattern = MagicContainer.GetBishopAttacks(pieceIndex, opt.OccupancySummary);
            pattern &= ~opt.FriendlyOccupancy;

            var excludeFromAttacks = pattern;
            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);
                var to = BitPositionConverter.ToPosition(patternIndex);

                if ((patternLSB & opt.EnemyOccupancy) == 0)
                {
                    opt.BitBoard.Moves.AddLast(new QuietMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }
                else
                {
                    opt.BitBoard.Moves.AddLast(new KillMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }

                if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                {
                    opt.BitBoard.Attacks[patternIndex] |= pieceLSB;
                    opt.BitBoard.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                }
            }

            return excludeFromAttacks;
        }

        private static void CalculateAttacks(PieceType pieceType, ulong pieceLSB, ulong movesPattern, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateAttacks) == 0)
            {
                return;
            }

            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
            var blockersToRemove = opt.BitBoard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Bishop)] |
                                   opt.BitBoard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Queen)];

            var piecesToParse = opt.BitBoard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];
            var allPiecesOccupancy = opt.OccupancySummary & ~blockersToRemove;

            var pattern = MagicContainer.GetBishopAttacks(pieceIndex, allPiecesOccupancy);
            pattern = CalculatePawnBlockers(pieceIndex, pattern, opt);
            pattern ^= movesPattern;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                opt.BitBoard.Attacks[patternIndex] |= pieceLSB;
                opt.BitBoard.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
            }
        }

        private static ulong CalculatePawnBlockers(int pieceIndex, ulong pattern, GeneratorParameters opt)
        {
            var patternWithFriendlyBlockers = pattern;
            var allowedBlockers = opt.BitBoard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];

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
                else
                if (opt.FriendlyColor == Color.White &&
                   (friendlyBlockerPosition.X < piecePosition.X && friendlyBlockerPosition.Y > piecePosition.Y) &&
                   (friendlyBlockerLSB & (BitConstants.AFile | BitConstants.HRank)) == 0)
                {
                    patternWithFriendlyBlockers |= friendlyBlockerLSB << 9;
                }

                else
                if (opt.FriendlyColor == Color.Black &&
                   (friendlyBlockerPosition.X > piecePosition.X && friendlyBlockerPosition.Y < piecePosition.Y) &&
                   (friendlyBlockerLSB & (BitConstants.HFile | BitConstants.ARank)) == 0)
                {
                    patternWithFriendlyBlockers |= friendlyBlockerLSB >> 9;
                }
                else
                if (opt.FriendlyColor == Color.Black &&
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