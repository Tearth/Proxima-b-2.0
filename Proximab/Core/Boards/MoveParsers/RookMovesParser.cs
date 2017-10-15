using Core.Boards.MoveGenerators;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using System.Collections.Generic;

namespace Core.Boards.MoveParsers
{
    class RookMovesParser : MovesParserBase
    {
        public RookMovesParser()
        {

        }

        public void GetMoves(PieceType pieceType, Color color, GeneratorMode mode, ulong[,] pieces, OccupancyContainer occupancyContainer, LinkedList<Move> moves, ulong[,] attacks)
        {
            var piecesToParse = pieces[(int)color, (int)pieceType];

            CalculateMoves(pieceType, color, mode, pieces, piecesToParse, occupancyContainer, moves);
            CalculateAttackFields(color, mode, pieces, piecesToParse, occupancyContainer, attacks);
        }

        void CalculateMoves(PieceType pieceType, Color color, GeneratorMode mode, ulong[,] pieces, ulong piecesToParse, OccupancyContainer occupancyContainer, LinkedList<Move> moves)
        {
            if (mode != GeneratorMode.CalculateAll)
                return;

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var horizontalPattern = GetHorizontalPattern(piecePosition, occupancyContainer.Occupancy);
                var verticalPattern = GetVerticalPattern(piecePosition, occupancyContainer.Occupancy);

                var pattern = (horizontalPattern | verticalPattern) & ~occupancyContainer.FriendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);
                    
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, occupancyContainer.EnemyOccupancy);

                    moves.AddLast(new Move(piecePosition, to, pieceType, color, moveType));
                }
            }
        }

        void CalculateAttackFields(Color color, GeneratorMode mode, ulong[,] pieces, ulong piecesToParse, OccupancyContainer occupancyContainer, ulong[,] attacks)
        {
            if (mode != GeneratorMode.CalculateAll && mode != GeneratorMode.CalculateAttackFields)
                return;

            var blockersToRemove = pieces[(int)color, (int)PieceType.Rook] |
                                   pieces[(int)color, (int)PieceType.Queen];

            var occupancyWithoutBlockers = occupancyContainer.Occupancy & ~blockersToRemove;
            
            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var horizontalPattern = GetHorizontalPattern(piecePosition, occupancyWithoutBlockers);
                var verticalPattern = GetVerticalPattern(piecePosition, occupancyWithoutBlockers);

                horizontalPattern = ExpandPatternByFriendlyPieces(color, Axis.Rank, horizontalPattern, pieces, occupancyWithoutBlockers);
                verticalPattern = ExpandPatternByFriendlyPieces(color, Axis.File, verticalPattern, pieces, occupancyWithoutBlockers);

                var pattern = horizontalPattern | verticalPattern;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }
        }

        ulong GetHorizontalPattern(Position piecePosition, ulong occupancy)
        {
            var offset = piecePosition.Y - 1;

            var pieceRank = (byte)(occupancy >> (offset * 8));
            var pattern = PredefinedMoves.SlideMoves[pieceRank, 8 - piecePosition.X];

            return (ulong)pattern << (offset * 8);
        }

        ulong GetVerticalPattern(Position piecePosition, ulong occupancy)
        {
            var offset = 8 - piecePosition.X;
            var rotatedOccupancy = BitOperations.Rotate90Right(occupancy);

            var pieceRank = (byte)(rotatedOccupancy >> (offset * 8));
            var pattern = PredefinedMoves.SlideMoves[pieceRank, 8 - piecePosition.Y];

            return BitOperations.Rotate90Left(pattern) << offset;
        }

        ulong ExpandPatternByFriendlyPieces(Color color, Axis axis, ulong pattern, ulong[,] pieces, ulong friendlyOccupancy)
        {
            var expandedPattern = pattern;

            var blockers = pattern & friendlyOccupancy;
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
                if ((blockerLSB & pieces[(int)color, (int)PieceType.King]) != 0)
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
