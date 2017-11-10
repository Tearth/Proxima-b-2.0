using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.MagicBitboards;
using Proxima.Core.MoveGenerators.PatternGenerators;
using System.Collections.Generic;

namespace Proxima.Core.MoveGenerators
{
    public class RookMovesGenerator
    {
        public RookMovesGenerator()
        {

        }

        public void Calculate(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);

                var patternContainer = CalculateMoves(pieceType, pieceLSB, opt);
                CalculateAttacks(pieceType, pieceLSB, patternContainer, opt);
            }
        }

        ulong CalculateMoves(PieceType pieceType, ulong pieceLSB, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
                return 0;

            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
            var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

            //var horizontalPattern = GetHorizontalPattern(piecePosition, opt.Occupancy) & ~opt.FriendlyOccupancy;
            //var verticalPattern = GetVerticalPattern(piecePosition, opt.Occupancy) & ~opt.FriendlyOccupancy;

            //var pattern = horizontalPattern | verticalPattern;
            var pattern = MagicBitboardsContainer.GetRookAttacks(pieceIndex, opt.Occupancy);
            pattern &= ~opt.FriendlyOccupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);
                    
                var to = BitPositionConverter.ToPosition(patternIndex);

                if ((patternLSB & opt.EnemyOccupancy) == 0)
                {
                    opt.Moves.AddLast(new QuietMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }
                else
                {
                    opt.Moves.AddLast(new KillMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }

                opt.Attacks[patternIndex] |= pieceLSB;
                opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
            }

            return pattern;
        }

        void CalculateAttacks(PieceType pieceType, ulong pieceLSB, ulong movesPattern, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateAttacks) == 0)
                return;

            var blockersToRemove = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Rook)] |
                                   opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Queen)];

            var occupancyWithoutBlockers = opt.Occupancy & ~blockersToRemove;
            
            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
            var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

            //var horizontalPattern = GetHorizontalPattern(piecePosition, occupancyWithoutBlockers);
            //var verticalPattern = GetVerticalPattern(piecePosition, occupancyWithoutBlockers);

            //horizontalPattern = ExpandPatternByFriendlyPieces(Axis.Rank, horizontalPattern, pieceLSB, opt);
            //horizontalPattern ^= patternContainer.Horizontal;

            //verticalPattern = ExpandPatternByFriendlyPieces(Axis.File, verticalPattern, pieceLSB, opt);
            //verticalPattern ^= patternContainer.Vertical;

            //var pattern = horizontalPattern | verticalPattern;
            var pattern = MagicBitboardsContainer.GetRookAttacks(pieceIndex, opt.Occupancy);
            pattern ^= movesPattern;
            pattern &= ~opt.FriendlyOccupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                opt.Attacks[patternIndex] |= pieceLSB;
                opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
            }
        }

        /*ulong GetHorizontalPattern(Position piecePosition, ulong occupancy)
        {
            var offset = piecePosition.Y - 1;

            var pieceRank = (byte)(occupancy >> (offset << 3));
            var pattern = PatternsContainer.SlidePattern[FastArray.GetSlideIndex(piecePosition.X, pieceRank)];
            
            return (ulong)pattern << (offset << 3);
        }

        ulong GetVerticalPattern(Position piecePosition, ulong occupancy)
        {
            var offset = 8 - piecePosition.X;
            var rotatedOccupancy = BitOperations.Rotate90Right(occupancy);

            var pieceRank = (byte)(rotatedOccupancy >> (offset << 3));
            var pattern = PatternsContainer.SlidePattern[FastArray.GetSlideIndex(piecePosition.Y, pieceRank)];

            return BitOperations.Rotate90Left(pattern) << offset;
        }

        /*ulong ExpandPatternByFriendlyPieces(Axis axis, ulong pattern, ulong pieceLSB, GeneratorParameters opt)
        {
            var expandedPattern = pattern;

            var blockers = pattern & opt.FriendlyOccupancy;

            var shift = 0;
            var mask = 0ul;

            if(axis == Axis.File)
            {
                mask = ~BitConstants.ARank & ~BitConstants.HRank;
                shift = 8;
            }
            else
            {
                mask = ~BitConstants.AFile & ~BitConstants.HFile;
                shift = 1;
            }

            while(blockers != 0)
            {
                var blockerLSB = BitOperations.GetLSB(ref blockers);
                if ((blockerLSB & opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.King)]) != 0)
                {
                    if(blockerLSB < pieceLSB)
                    {
                        expandedPattern |= (blockerLSB & mask) >> shift;
                    }
                    else
                    {
                        expandedPattern |= (blockerLSB & mask) << shift;
                    }
                }
            }

            return expandedPattern;
        }*/
    }
}
