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

                var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(allPiecesOccupancy, pieceLSB);
                var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(allPiecesOccupancy, pieceLSB);

                var pattern = (rightRotatedBitBoardPattern | leftRotatedBitBoardPattern) & ~friendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, enemyOccupancy);

                    moves.Add(new Move(from, to, pieceType, color, moveType));
                }
            }

            return moves;
        }

        void CalculateAttackFields(ulong[,] pieces, ulong piecesToParse, Color color, ulong[] occupancy, ref ulong[,] attacks)
        {
            var friendlyOccupancy = occupancy[(int)color];
            var enemyOccupancy = occupancy[(int)ColorOperations.Invert(color)];

            var blockersToRemove = pieces[(int)color, (int)PieceType.Bishop] |
                                   pieces[(int)color, (int)PieceType.Queen];

            var allPiecesOccupancy = (friendlyOccupancy & ~blockersToRemove) | enemyOccupancy;

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(allPiecesOccupancy, pieceLSB);
                var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(allPiecesOccupancy, pieceLSB);

                rightRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(rightRotatedBitBoardPattern, pieces, friendlyOccupancy, Diagonal.A8H1, color);
                leftRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(leftRotatedBitBoardPattern, pieces, friendlyOccupancy, Diagonal.A1H8, color);

                var pattern = (rightRotatedBitBoardPattern | leftRotatedBitBoardPattern) & ~friendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }
        }

        ulong GetRightRotatedBitBoardPattern(ulong occupancy, ulong pieceLSB)
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

        ulong GetLeftRotatedBitBoardPattern(ulong occupancy, ulong pieceLSB)
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

        ulong ExpandPatternByFriendlyPieces(ulong pattern, ulong[,] pieces, ulong friendlyOccupation, Diagonal diagonal, Color color)
        {
            var expandedPattern = pattern;

            var blockers = pattern & friendlyOccupation;
            var patternLSB = BitOperations.GetLSB(ref pattern);

            var shift = 0;
            var mask = ~BitConstants.ARank & ~BitConstants.AFile & ~BitConstants.HRank & ~BitConstants.HFile;

            if (diagonal == Diagonal.A1H8)
            {
                shift = 9;
            }
            else if (diagonal == Diagonal.A8H1)
            {
                shift = 7;
            }

            while (blockers != 0)
            {
                var blockerLSB = BitOperations.GetLSB(ref blockers);
                if ((blockerLSB & pieces[(int)color, (int)PieceType.King]) != 0 ||
                    (blockerLSB & pieces[(int)color, (int)PieceType.Pawn]) != 0)
                {
                    if (blockerLSB == patternLSB)
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