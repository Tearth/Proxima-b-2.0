using Core.Boards.MoveGenerators;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using System;
using System.Collections.Generic;

namespace Core.Boards.MoveParsers
{
    public class BishopMovesParser : MovesParserBase
    {
        public BishopMovesParser()
        {

        }

        public List<Move> GetMoves(PieceType pieceType, Color color, ulong[,] pieces, OccupancyContainer occupancyContainer, ref ulong[,] attacks)
        {
            var piecesToParse = pieces[(int)color, (int)pieceType];
            var moves = CalculateMoves(pieceType, color, pieces, piecesToParse, occupancyContainer);

            CalculateAttackFields(color, pieces, piecesToParse, occupancyContainer, ref attacks);

            return moves;
        }

        List<Move> CalculateMoves(PieceType pieceType, Color color, ulong[,] pieces, ulong piecesToParse, OccupancyContainer occupancyContainer)
        {
            var moves = new List<Move>();

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(pieceLSB, occupancyContainer);
                var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(pieceLSB, occupancyContainer);

                var pattern = (rightRotatedBitBoardPattern | leftRotatedBitBoardPattern) & ~occupancyContainer.FriendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, occupancyContainer.EnemyOccupancy);

                    moves.Add(new Move(from, to, pieceType, color, moveType));
                }
            }

            return moves;
        }

        void CalculateAttackFields(Color color, ulong[,] pieces, ulong piecesToParse, OccupancyContainer occupancyContainer, ref ulong[,] attacks)
        {
            var blockersToRemove = pieces[(int)color, (int)PieceType.Bishop] |
                                   pieces[(int)color, (int)PieceType.Queen];

            var allPiecesOccupancy = occupancyContainer.Occupancy & ~blockersToRemove;

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(pieceLSB, occupancyContainer);
                var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(pieceLSB, occupancyContainer);

                rightRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(color, Diagonal.A8H1, rightRotatedBitBoardPattern, pieces, occupancyContainer);
                leftRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(color, Diagonal.A1H8, leftRotatedBitBoardPattern, pieces, occupancyContainer);

                var pattern = (rightRotatedBitBoardPattern | leftRotatedBitBoardPattern) & ~occupancyContainer.FriendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }
        }

        ulong GetRightRotatedBitBoardPattern(ulong pieceLSB, OccupancyContainer occupancyContainer)
        {
            var rotatedOccupancy = BitOperations.Rotate45Right(occupancyContainer.Occupancy);
            var rotatedPieceLSB = BitOperations.Rotate45Right(pieceLSB);
            var rotatedPiecePosition = BitPositionConverter.ToPosition(rotatedPieceLSB);

            var mask = (byte)(Math.Pow(2, rotatedPiecePosition.Y - 1) - 1);

            if((rotatedPieceLSB & BitConstants.LeftBottomBoardPart) != 0)
            {
                mask ^= 0xFF;
            }

            var pieceRank = (byte)(rotatedOccupancy >> ((rotatedPiecePosition.Y - 1) * 8));
            var availableMoves = PredefinedMoves.SlideMoves[pieceRank, 8 - rotatedPiecePosition.X] & mask;

            return BitOperations.Rotate45Left((ulong)availableMoves << ((rotatedPiecePosition.Y - 2) * 8));
        }

        ulong GetLeftRotatedBitBoardPattern(ulong pieceLSB, OccupancyContainer occupancyContainer)
        {
            var rotatedOccupancy = BitOperations.Rotate45Left(occupancyContainer.Occupancy);
            var rotatedPieceLSB = BitOperations.Rotate45Left(pieceLSB);
            var rotatedPiecePosition = BitPositionConverter.ToPosition(rotatedPieceLSB);

            var mask = (byte)(Math.Pow(2, 8 - rotatedPiecePosition.Y + 1) - 1);

            if ((rotatedPieceLSB & BitConstants.LeftTopBoardPart) != 0)
            {
                mask ^= 0xFF;
            }

            var pieceRank = (byte)(rotatedOccupancy >> ((rotatedPiecePosition.Y - 1) * 8));
            var availableMoves = PredefinedMoves.SlideMoves[pieceRank, 8 - rotatedPiecePosition.X] & mask;

            return BitOperations.Rotate45Right((ulong)availableMoves << ((rotatedPiecePosition.Y - 2) * 8));
        }

        ulong ExpandPatternByFriendlyPieces(Color color, Diagonal diagonal, ulong pattern, ulong[,] pieces, OccupancyContainer occupancyContainer)
        {
            var expandedPattern = pattern;

            var blockers = pattern & occupancyContainer.FriendlyOccupancy;
            var patternLSB = BitOperations.GetLSB(ref pattern);

            var shift = diagonal == Diagonal.A1H8 ? 9 : 7;
            var mask = ~BitConstants.ARank & ~BitConstants.AFile & ~BitConstants.HRank & ~BitConstants.HFile;

            while (blockers != 0)
            {
                var blockerLSB = BitOperations.GetLSB(ref blockers);
                var kingBlocker = pieces[(int)color, (int)PieceType.King];
                var pawnBlocker = pieces[(int)color, (int)PieceType.Pawn];

                if ((blockerLSB & (kingBlocker | pawnBlocker)) != 0)
                {
                    if (blockerLSB == patternLSB)
                    {
                        if (pawnBlocker == 0 || (pawnBlocker != 0 && color == Color.Black))
                        {
                            expandedPattern |= (blockerLSB & mask) >> shift;
                        }
                    }
                    else
                    {
                        if(pawnBlocker == 0 || (pawnBlocker != 0 && color == Color.White))
                        {
                            expandedPattern |= (blockerLSB & mask) << shift;
                        }
                    }
                }
            }

            return expandedPattern;
        }
    }
}