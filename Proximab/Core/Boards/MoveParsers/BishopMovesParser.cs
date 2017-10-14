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

        public void GetMoves(PieceType pieceType, Color color, GeneratorMode mode, ulong[,] pieces, OccupancyContainer occupancyContainer, LinkedList<Move> moves, ref ulong[,] attacks)
        {
            var piecesToParse = pieces[(int)color, (int)pieceType];

            CalculateMoves(pieceType, color, mode, pieces, piecesToParse, occupancyContainer, moves);
            CalculateAttackFields(color, mode, pieces, piecesToParse, occupancyContainer, ref attacks);
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

                var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(pieceLSB, occupancyContainer.Occupancy);
                var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(pieceLSB, occupancyContainer.Occupancy);

                var pattern = (rightRotatedBitBoardPattern | leftRotatedBitBoardPattern) & ~occupancyContainer.FriendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, occupancyContainer.EnemyOccupancy);

                    moves.AddLast(new Move(from, to, pieceType, color, moveType));
                }
            }
        }

        void CalculateAttackFields(Color color, GeneratorMode mode, ulong[,] pieces, ulong piecesToParse, OccupancyContainer occupancyContainer, ref ulong[,] attacks)
        {
            if (mode != GeneratorMode.CalculateAll && mode != GeneratorMode.CalculateAttackFields)
                return;

            var blockersToRemove = pieces[(int)color, (int)PieceType.Bishop] |
                                   pieces[(int)color, (int)PieceType.Queen];

            var allPiecesOccupancy = occupancyContainer.Occupancy & ~blockersToRemove;

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(pieceLSB, allPiecesOccupancy);
                var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(pieceLSB, allPiecesOccupancy);

                rightRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(color, Diagonal.A8H1, pieceLSB, rightRotatedBitBoardPattern, pieces, allPiecesOccupancy);
                leftRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(color, Diagonal.A1H8, pieceLSB, leftRotatedBitBoardPattern, pieces, allPiecesOccupancy);

                var pattern = (rightRotatedBitBoardPattern | leftRotatedBitBoardPattern) & ~occupancyContainer.FriendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }
        }

        ulong GetRightRotatedBitBoardPattern(ulong pieceLSB, ulong occupancy)
        {
            var rotatedOccupancy = BitOperations.Rotate45Right(occupancy);
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

        ulong GetLeftRotatedBitBoardPattern(ulong pieceLSB, ulong occupancy)
        {
            var rotatedOccupancy = BitOperations.Rotate45Left(occupancy);
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

        ulong ExpandPatternByFriendlyPieces(Color color, Diagonal diagonal, ulong pieceLSB, ulong pattern, ulong[,] pieces, ulong friendlyOccupancy)
        {
            var expandedPattern = pattern;

            var blockers = pattern & friendlyOccupancy;

            var shift = diagonal == Diagonal.A1H8 ? 9 : 7;
            var mask = ~BitConstants.ARank & ~BitConstants.AFile & ~BitConstants.HRank & ~BitConstants.HFile;

            while (blockers != 0)
            {
                var blockerLSB = BitOperations.GetLSB(ref blockers);
                var kingBlockers = pieces[(int)color, (int)PieceType.King];
                var pawnBlockers = pieces[(int)color, (int)PieceType.Pawn];

                if ((blockerLSB & (kingBlockers | pawnBlockers)) != 0)
                {
                    if (blockerLSB < pieceLSB)
                    {
                        if (pawnBlockers == 0 || (pawnBlockers != 0 && color == Color.Black))
                        {
                            expandedPattern |= (blockerLSB & mask) >> shift;
                        }
                    }
                    else
                    {
                        if(pawnBlockers == 0 || (pawnBlockers != 0 && color == Color.White))
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