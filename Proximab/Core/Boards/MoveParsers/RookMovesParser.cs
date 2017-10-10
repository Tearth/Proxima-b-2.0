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

        public List<Move> GetMoves(Color color, PieceType pieceType, ulong[,] pieces, ulong[] occupancy, ref ulong[,] attacks)
        {
            var piecesToParse = pieces[(int)color, (int)pieceType];
            var moves = CalculateMoves(pieces, piecesToParse, occupancy, pieceType, color);

            CalculateAttackFields(pieces, piecesToParse, color, occupancy, ref attacks);

            return moves;
        }

        List<Move> CalculateMoves(ulong[,] pieces, ulong piecesToParse, ulong[] occupancy, PieceType pieceType, Color color)
        {
            var friendlyOccupancy = occupancy[(int)color];
            var enemyOccupancy = occupancy[(int)ColorOperations.Invert(color)];
            var allPiecesOccupancy = friendlyOccupancy | enemyOccupancy;

            var moves = new List<Move>();

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var horizontalPattern = GetHorizontalPattern(allPiecesOccupancy, piecePosition);
                var verticalPattern = GetVerticalPattern(allPiecesOccupancy, piecePosition);

                var pattern = (horizontalPattern | verticalPattern) & ~friendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);
                    
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, enemyOccupancy);

                    moves.Add(new Move(piecePosition, to, pieceType, color, moveType));
                }
            }

            return moves;
        }

        void CalculateAttackFields(ulong[,] pieces, ulong piecesToParse, Color color, ulong[] occupancy, ref ulong[,] attacks)
        {
            var friendlyOccupancy = occupancy[(int)color];
            var enemyOccupancy = occupancy[(int)ColorOperations.Invert(color)];

            var blockersToRemove = pieces[(int)color, (int)PieceType.Rook] |
                                   pieces[(int)color, (int)PieceType.Queen];

            var allPiecesOccupancy = (friendlyOccupancy & ~blockersToRemove) | enemyOccupancy;

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var horizontalPattern = GetHorizontalPattern(allPiecesOccupancy, piecePosition);
                var verticalPattern = GetVerticalPattern(allPiecesOccupancy, piecePosition);

                horizontalPattern = ExpandPatternByFriendlyPieces(horizontalPattern, pieces, friendlyOccupancy, Axis.Rank, color);
                verticalPattern = ExpandPatternByFriendlyPieces(verticalPattern, pieces, friendlyOccupancy, Axis.File, color);

                var pattern = horizontalPattern | verticalPattern;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }
        }

        ulong GetHorizontalPattern(ulong occupancy, Position piecePosition)
        {
            var offset = piecePosition.Y - 1;

            var pieceRank = (byte)(occupancy >> (offset * 8));
            var pattern = PredefinedMoves.SlideMoves[pieceRank, 8 - piecePosition.X];

            return (ulong)pattern << (offset * 8);
        }

        ulong GetVerticalPattern(ulong occupancy, Position piecePosition)
        {
            var offset = 8 - piecePosition.X;
            var rotatedOccupancy = BitOperations.Rotate90Right(occupancy);

            var pieceRank = (byte)(rotatedOccupancy >> (offset * 8));
            var pattern = PredefinedMoves.SlideMoves[pieceRank, 8 - piecePosition.Y];

            return BitOperations.Rotate90Left(pattern) << offset;
        }

        ulong ExpandPatternByFriendlyPieces(ulong pattern, ulong[,] pieces, ulong friendlyOccupation, Axis axis, Color color)
        {
            var expandedPattern = pattern;

            var blockers = pattern & friendlyOccupation;
            var patternLSB = BitOperations.GetLSB(ref pattern);

            var shift = 0;
            var mask = 0ul;

            if(axis == Axis.File)
            {
                shift = 8;
                mask = ~BitConstants.ARank & ~BitConstants.HRank;
            }
            else if(axis == Axis.Rank)
            {
                shift = 1;
                mask = ~BitConstants.AFile & ~BitConstants.HFile;
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
