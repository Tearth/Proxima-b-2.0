using Core.Boards.PatternGenerators;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using System;
using System.Collections.Generic;

namespace Core.Boards.MoveGenerators
{
    public class BishopMovesGenerator : MovesParserBase
    {
        public BishopMovesGenerator()
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

                (ulong leftPattern, ulong rightPattern) = CalculateMoves(pieceType, pieceLSB, opt);
                CalculateAttack(pieceType, pieceLSB, leftPattern, rightPattern, opt);
            }
        }

        (ulong leftPattern, ulong rightPattern) CalculateMoves(PieceType pieceType, ulong pieceLSB, GeneratorParameters opt)
        {
            if (opt.Mode != GeneratorMode.CalculateAll)
                return (0, 0);

            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

            var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(pieceLSB, opt.Occupancy);
            var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(pieceLSB, opt.Occupancy);

            var pattern = (rightRotatedBitBoardPattern | leftRotatedBitBoardPattern) & ~opt.FriendlyOccupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var from = BitPositionConverter.ToPosition(pieceIndex);
                var to = BitPositionConverter.ToPosition(patternIndex);
                var moveType = GetMoveType(patternLSB, opt.EnemyOccupancy);

                opt.Moves.AddLast(new Move(from, to, pieceType, opt.Color, moveType));
                opt.Attacks[(int)opt.Color, patternIndex] |= pieceLSB;
            }

            return (leftRotatedBitBoardPattern, rightRotatedBitBoardPattern);
        }

        void CalculateAttack(PieceType pieceType, ulong pieceLSB, ulong leftPattern, ulong rightPattern, GeneratorParameters opt)
        {
            if (opt.Mode != GeneratorMode.CalculateAll && opt.Mode != GeneratorMode.CalculateAttackFields)
                return;

            var blockersToRemove = opt.Pieces[(int)opt.Color, (int)PieceType.Bishop] |
                                   opt.Pieces[(int)opt.Color, (int)PieceType.Queen];

            var piecesToParse = opt.Pieces[(int)opt.Color, (int)pieceType];
            var allPiecesOccupancy = opt.Occupancy & ~blockersToRemove;

            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

            var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(pieceLSB, allPiecesOccupancy);
            var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(pieceLSB, allPiecesOccupancy);

            rightRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(Diagonal.A8H1, rightRotatedBitBoardPattern, opt) ^ rightPattern;
            leftRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(Diagonal.A1H8, leftRotatedBitBoardPattern, opt) ^ leftPattern;

            var pattern = (rightRotatedBitBoardPattern | leftRotatedBitBoardPattern) & ~opt.FriendlyOccupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                opt.Attacks[(int)opt.Color, patternIndex] |= pieceLSB;
            }
        }

        ulong GetRightRotatedBitBoardPattern(ulong pieceLSB, ulong occupancy)
        {
            var rotatedOccupancy = BitOperations.Rotate45Right(occupancy);
            var rotatedPieceLSB = BitOperations.Rotate45Right(pieceLSB);
            var rotatedPieceIndex = BitOperations.GetBitIndex(rotatedPieceLSB);
            var rotatedPiecePosition = BitPositionConverter.ToPosition(rotatedPieceIndex);

            var mask = (1 << (rotatedPiecePosition.Y - 1)) - 1;

            if((rotatedPieceLSB & BitConstants.LeftBottomBoardPart) != 0)
            {
                mask ^= 0xFF;
            }

            var pieceRank = (byte)(rotatedOccupancy >> ((rotatedPiecePosition.Y - 1) * 8));
            var availableMoves = PatternsContainer.SlidePattern[pieceRank, 8 - rotatedPiecePosition.X] & mask;

            return BitOperations.Rotate45Left((ulong)availableMoves << ((rotatedPiecePosition.Y - 2) * 8));
        }

        ulong GetLeftRotatedBitBoardPattern(ulong pieceLSB, ulong occupancy)
        {
            var rotatedOccupancy = BitOperations.Rotate45Left(occupancy);
            var rotatedPieceLSB = BitOperations.Rotate45Left(pieceLSB);
            var rotatedPieceIndex = BitOperations.GetBitIndex(rotatedPieceLSB);
            var rotatedPiecePosition = BitPositionConverter.ToPosition(rotatedPieceIndex);

            var mask = (1 << (8 - rotatedPiecePosition.Y + 1)) - 1;

            if ((rotatedPieceLSB & BitConstants.LeftTopBoardPart) != 0)
            {
                mask ^= 0xFF;
            }

            var pieceRank = (byte)(rotatedOccupancy >> ((rotatedPiecePosition.Y - 1) * 8));
            var availableMoves = PatternsContainer.SlidePattern[pieceRank, 8 - rotatedPiecePosition.X] & mask;

            return BitOperations.Rotate45Right((ulong)availableMoves << ((rotatedPiecePosition.Y - 2) * 8));
        }

        ulong ExpandPatternByFriendlyPieces(Diagonal diagonal, ulong pattern, GeneratorParameters opt)
        {
            var expandedPattern = pattern;

            var blockers = pattern & opt.FriendlyOccupancy;
            var patternLSB = BitOperations.GetLSB(ref pattern);

            var shift = diagonal == Diagonal.A1H8 ? 9 : 7;
            var mask = ~BitConstants.ARank & ~BitConstants.AFile & ~BitConstants.HRank & ~BitConstants.HFile;

            while (blockers != 0)
            {
                var blockerLSB = BitOperations.GetLSB(ref blockers);
                var kingBlockers = opt.Pieces[(int)opt.Color, (int)PieceType.King];
                var pawnBlockers = opt.Pieces[(int)opt.Color, (int)PieceType.Pawn];

                if ((blockerLSB & (kingBlockers | pawnBlockers)) != 0)
                {
                    if (blockerLSB == patternLSB)
                    {
                        if (pawnBlockers == 0 || (pawnBlockers != 0 && opt.Color == Color.Black))
                        {
                            expandedPattern |= (blockerLSB & mask) >> shift;
                        }
                    }
                    else
                    {
                        if(pawnBlockers == 0 || (pawnBlockers != 0 && opt.Color == Color.White))
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