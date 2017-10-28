using Proxima.Core.Boards.PatternGenerators;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Boards.MoveGenerators
{
    public class BishopMovesGenerator
    {
        public BishopMovesGenerator()
        {

        }

        public void Calculate(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];
            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);

                var patternContainer = CalculateMoves(pieceType, pieceLSB, opt);
                CalculateAttack(pieceType, pieceLSB, patternContainer, opt);
            }
        }

        BishopPatternContainer CalculateMoves(PieceType pieceType, ulong pieceLSB, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
                return new BishopPatternContainer();

            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
            var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

            var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(pieceLSB, opt.Occupancy) & ~opt.FriendlyOccupancy;
            var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(pieceLSB, opt.Occupancy) & ~opt.FriendlyOccupancy;

            var pattern = rightRotatedBitBoardPattern | leftRotatedBitBoardPattern;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var to = BitPositionConverter.ToPosition(patternIndex);

                if((patternLSB & opt.EnemyOccupancy) == 0)
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

            return new BishopPatternContainer(rightRotatedBitBoardPattern, leftRotatedBitBoardPattern);
        }

        void CalculateAttack(PieceType pieceType, ulong pieceLSB, BishopPatternContainer patternContainer, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateAttacks) == 0)
                return;

            var blockersToRemove = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Bishop)] |
                                   opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Queen)];

            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];
            var allPiecesOccupancy = opt.Occupancy & ~blockersToRemove;

            var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(pieceLSB, allPiecesOccupancy);
            var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(pieceLSB, allPiecesOccupancy);

            rightRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(Diagonal.A1H8, rightRotatedBitBoardPattern, pieceLSB, opt);
            rightRotatedBitBoardPattern ^= patternContainer.A1H8Diagonal;

            leftRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(Diagonal.A8H1, leftRotatedBitBoardPattern, pieceLSB, opt);
            leftRotatedBitBoardPattern ^= patternContainer.A8H1Diagonal;

            var pattern = rightRotatedBitBoardPattern | leftRotatedBitBoardPattern;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                opt.Attacks[patternIndex] |= pieceLSB;
                opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
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

            var pieceRank = (byte)(rotatedOccupancy >> ((rotatedPiecePosition.Y - 1) << 3));
            var availableMoves = PatternsContainer.SlidePattern[FastArray.GetSlideIndex(rotatedPiecePosition.X, pieceRank)] & mask;

            return BitOperations.Rotate45Left((ulong)availableMoves << ((rotatedPiecePosition.Y - 2) << 3));
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

            var pieceRank = (byte)(rotatedOccupancy >> ((rotatedPiecePosition.Y - 1) << 3));
            var availableMoves = PatternsContainer.SlidePattern[FastArray.GetSlideIndex(rotatedPiecePosition.X, pieceRank)] & mask;

            return BitOperations.Rotate45Right((ulong)availableMoves << ((rotatedPiecePosition.Y - 2) << 3));
        }

        ulong ExpandPatternByFriendlyPieces(Diagonal diagonal, ulong pattern, ulong pieceLSB, GeneratorParameters opt)
        {
            var expandedPattern = pattern;

            var blockers = pattern & opt.FriendlyOccupancy;

            var shift = diagonal == Diagonal.A1H8 ? 7 : 9;
            var mask = ~BitConstants.ARank & ~BitConstants.AFile & ~BitConstants.HRank & ~BitConstants.HFile;

            while (blockers != 0)
            {
                var blockerLSB = BitOperations.GetLSB(ref blockers);
                var kingBlockers = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.King)];
                var pawnBlockers = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];

                if ((blockerLSB & (kingBlockers | pawnBlockers)) != 0)
                {
                    if (blockerLSB < pieceLSB)
                    {
                        if (pawnBlockers == 0 || (pawnBlockers != 0 && opt.FriendlyColor == Color.Black))
                        {
                            expandedPattern |= (blockerLSB & mask) >> shift;
                        }
                    }
                    else
                    {
                        if(pawnBlockers == 0 || (pawnBlockers != 0 && opt.FriendlyColor == Color.White))
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