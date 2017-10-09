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
        BitBoard _bitBoard;

        public RookMovesParser(BitBoard bitBoard)
        {
            _bitBoard = bitBoard;
        }

        public List<Move> GetMoves(Color color)
        {
            var friendlyOccupancy = _bitBoard.Occupancy[(int)color];
            var enemyOccupancy = _bitBoard.Occupancy[(int)ColorOperations.Invert(color)];
            var occupancy = friendlyOccupancy | enemyOccupancy;

            var pieces = _bitBoard.Pieces[(int)color, (int)PieceType.Rook];
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

                var horizontalPattern = GetHorizontalPattern(occupancy, piecePosition, false);
                var verticalPattern = GetVerticalPattern(occupancy, piecePosition, false);

                var pattern = (horizontalPattern | verticalPattern) & ~friendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, enemyOccupancy);

                    moves.Add(new Move(from, to, PieceType.Rook, color, moveType));
                }
            }

            return moves;
        }

        void CalculateAttackFields(ulong pieces, Color color, ulong friendlyOccupancy, ulong enemyOccupancy)
        {
            var blockersToRemove = _bitBoard.Pieces[(int)color, (int)PieceType.Rook] |
                                   _bitBoard.Pieces[(int)color, (int)PieceType.Queen];

            var occupancy = (friendlyOccupancy & ~blockersToRemove) | enemyOccupancy;

            while (pieces != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref pieces);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var horizontalPattern = GetHorizontalPattern(occupancy, piecePosition, true);
                var verticalPattern = GetVerticalPattern(occupancy, piecePosition, true);

                horizontalPattern = ExpandPatternByFriendlyPieces(horizontalPattern, friendlyOccupancy, Axis.Rank, color);
                verticalPattern = ExpandPatternByFriendlyPieces(verticalPattern, friendlyOccupancy, Axis.File, color);

                var pattern = horizontalPattern | verticalPattern;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    _bitBoard.Attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }
        }

        ulong GetHorizontalPattern(ulong occupancy, Position piecePosition, bool includeFriendlyBlockerMoves)
        {
            var offset = piecePosition.Y - 1;

            var pieceRank = (byte)(occupancy >> (offset * 8));
            var pattern = PredefinedMoves.Rook[pieceRank, 8 - piecePosition.X];

            return (ulong)pattern << (offset * 8);
        }

        ulong GetVerticalPattern(ulong occupancy, Position piecePosition, bool includeFriendlyBlockerMoves)
        {
            var offset = 8 - piecePosition.X;
            var rotatedOccupancy = BitOperations.RotateRight(occupancy);

            var pieceRank = (byte)(rotatedOccupancy >> (offset * 8));
            var pattern = PredefinedMoves.Rook[pieceRank, 8 - piecePosition.Y];

            return BitOperations.RotateLeft(pattern) << offset;
        }

        ulong ExpandPatternByFriendlyPieces(ulong pattern, ulong friendlyOccupation, Axis axis, Color color)
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
                if ((blockerLSB & _bitBoard.Pieces[(int)color, (int)PieceType.King]) != 0)
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
