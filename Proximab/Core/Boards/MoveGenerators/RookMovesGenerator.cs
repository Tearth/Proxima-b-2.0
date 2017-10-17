using Core.Boards.PatternGenerators;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using System.Collections.Generic;

namespace Core.Boards.MoveGenerators
{
    class RookMovesGenerator : MovesParserBase
    {
        public RookMovesGenerator()
        {

        }

        public void GetMoves(PieceType pieceType, GeneratorParameters opt)
        {
            Calculate(pieceType, opt);
        }

        void Calculate(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[(int)opt.Color, (int)pieceType];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);

                (ulong horizontalPattern, ulong verticalPattern) = CalculateMoves(pieceType, pieceLSB, opt);
                CalculateAttacks(pieceType, pieceLSB, horizontalPattern, verticalPattern, opt);
            }
        }

        (ulong horizontalMovesPattern, ulong verticalMovesPattern) CalculateMoves(PieceType pieceType, ulong pieceLSB, GeneratorParameters opt)
        {
            if (opt.Mode != GeneratorMode.CalculateAll)
                return (0, 0);

            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
            var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

            var horizontalPattern = GetHorizontalPattern(piecePosition, opt.Occupancy);
            var verticalPattern = GetVerticalPattern(piecePosition, opt.Occupancy);

            var pattern = (horizontalPattern | verticalPattern) & ~opt.FriendlyOccupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);
                    
                var to = BitPositionConverter.ToPosition(patternIndex);
                var moveType = GetMoveType(patternLSB, opt.EnemyOccupancy);

                opt.Moves.AddLast(new Move(piecePosition, to, pieceType, opt.Color, moveType));
            }

            return (horizontalPattern, verticalPattern);
        }

        void CalculateAttacks(PieceType pieceType, ulong pieceLSB, ulong movesHorizontalPattern, ulong movesVerticalPattern, GeneratorParameters opt)
        {
            if (opt.Mode != GeneratorMode.CalculateAll && opt.Mode != GeneratorMode.CalculateAttackFields)
                return;

            var blockersToRemove = opt.Pieces[(int)opt.Color, (int)PieceType.Rook] |
                                   opt.Pieces[(int)opt.Color, (int)PieceType.Queen];

            var occupancyWithoutBlockers = opt.Occupancy & ~blockersToRemove;
            
            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
            var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

            var horizontalPattern = GetHorizontalPattern(piecePosition, occupancyWithoutBlockers);
            var verticalPattern = GetVerticalPattern(piecePosition, occupancyWithoutBlockers);

            horizontalPattern = ExpandPatternByFriendlyPieces(Axis.Rank, horizontalPattern, opt) ^ movesHorizontalPattern;
            verticalPattern = ExpandPatternByFriendlyPieces(Axis.File, verticalPattern, opt) ^ movesVerticalPattern;

            var pattern = horizontalPattern | verticalPattern;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                opt.Attacks[(int)opt.Color, patternIndex] |= pieceLSB;
            }
        }

        ulong GetHorizontalPattern(Position piecePosition, ulong occupancy)
        {
            var offset = piecePosition.Y - 1;

            var pieceRank = (byte)(occupancy >> (offset * 8));
            var pattern = PatternsContainer.SlidePattern[pieceRank, 8 - piecePosition.X];

            return (ulong)pattern << (offset * 8);
        }

        ulong GetVerticalPattern(Position piecePosition, ulong occupancy)
        {
            var offset = 8 - piecePosition.X;
            var rotatedOccupancy = BitOperations.Rotate90Right(occupancy);

            var pieceRank = (byte)(rotatedOccupancy >> (offset * 8));
            var pattern = PatternsContainer.SlidePattern[pieceRank, 8 - piecePosition.Y];

            return BitOperations.Rotate90Left(pattern) << offset;
        }

        ulong ExpandPatternByFriendlyPieces(Axis axis, ulong pattern, GeneratorParameters opt)
        {
            var expandedPattern = pattern;

            var blockers = pattern & opt.FriendlyOccupancy;
            var patternLSB = BitOperations.GetLSB(ref pattern);

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
                if ((blockerLSB & opt.Pieces[(int)opt.Color, (int)PieceType.King]) != 0)
                {
                    if(blockerLSB == patternLSB)
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
        }
    }
}
