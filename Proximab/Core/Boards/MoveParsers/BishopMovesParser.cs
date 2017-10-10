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
        BitBoard _bitBoard;
        
        public BishopMovesParser(BitBoard bitBoard)
        {
            _bitBoard = bitBoard;
        }

        public List<Move> GetMoves(Color color)
        {
            var friendlyOccupancy = _bitBoard.Occupancy[(int)color];
            var enemyOccupancy = _bitBoard.Occupancy[(int)ColorOperations.Invert(color)];

            var pieces = _bitBoard.Pieces[(int)color, (int)PieceType.Bishop];
            var moves = CalculateMoves(pieces, color, friendlyOccupancy, enemyOccupancy);

            CalculateAttackFields(pieces, color, friendlyOccupancy, enemyOccupancy);

            return moves;
        }

        List<Move> CalculateMoves(ulong pieces, Color color, ulong friendlyOccupancy, ulong enemyOccupancy)
        {
            var moves = new List<Move>();
            var occupancy = friendlyOccupancy | enemyOccupancy;

            while (pieces != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref pieces);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(occupancy, pieceLSB);
                var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(occupancy, pieceLSB);

                var pattern = (rightRotatedBitBoardPattern | leftRotatedBitBoardPattern) & ~friendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, enemyOccupancy);

                    moves.Add(new Move(from, to, PieceType.Bishop, color, moveType));
                }
            }

            return moves;
        }

        void CalculateAttackFields(ulong pieces, Color color, ulong friendlyOccupancy, ulong enemyOccupancy)
        {
            var blockersToRemove = _bitBoard.Pieces[(int)color, (int)PieceType.Bishop] |
                                   _bitBoard.Pieces[(int)color, (int)PieceType.Queen];

            var occupancy = (friendlyOccupancy & ~blockersToRemove) | enemyOccupancy;

            while (pieces != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref pieces);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var rightRotatedBitBoardPattern = GetRightRotatedBitBoardPattern(occupancy, pieceLSB);
                var leftRotatedBitBoardPattern = GetLeftRotatedBitBoardPattern(occupancy, pieceLSB);

                rightRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(rightRotatedBitBoardPattern, friendlyOccupancy, Diagonal.A8H1, color);
                leftRotatedBitBoardPattern = ExpandPatternByFriendlyPieces(leftRotatedBitBoardPattern, friendlyOccupancy, Diagonal.A1H8, color);

                var pattern = (rightRotatedBitBoardPattern | leftRotatedBitBoardPattern) & ~friendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    _bitBoard.Attacks[(int)color, patternIndex] |= pieceLSB;
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

        ulong ExpandPatternByFriendlyPieces(ulong pattern, ulong friendlyOccupation, Diagonal diagonal, Color color)
        {
            var expandedPattern = pattern;

            var blockers = pattern & friendlyOccupation;
            var patternLSB = BitOperations.GetLSB(ref pattern);

            var shift = 0;
            var mask = 0xFFul;

            if (diagonal == Diagonal.A1H8)
            {
                shift = 9;
                mask = ~BitConstants.HRank & ~BitConstants.HFile;
            }
            else if (diagonal == Diagonal.A8H1)
            {
                shift = 7;
                mask = ~BitConstants.AFile & ~BitConstants.ARank;
            }

            while (blockers != 0)
            {
                var blockerLSB = BitOperations.GetLSB(ref blockers);
                if ((blockerLSB & _bitBoard.Pieces[(int)color, (int)PieceType.King]) != 0 ||
                    (blockerLSB & _bitBoard.Pieces[(int)color, (int)PieceType.Pawn]) != 0)
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