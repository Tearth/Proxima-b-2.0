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

            CalculateXRay(pieces, color, friendlyOccupancy, enemyOccupancy);

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

                var horizontalPattern = GetHorizontalPattern(occupancy, piecePosition);
                var verticalPattern = GetVerticalPattern(occupancy, piecePosition);

                var pattern = (horizontalPattern | verticalPattern) & ~friendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, enemyOccupancy);

                    moves.Add(new Move(from, to, PieceType.Rook, color, moveType));

                    _bitBoard.Attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }

            return moves;
        }

        void CalculateXRay(ulong pieces, Color color, ulong friendlyOccupancy, ulong enemyOccupancy)
        {
            var blockersToRemove = _bitBoard.Pieces[(int)color, (int)PieceType.Rook] |
                                   _bitBoard.Pieces[(int)color, (int)PieceType.Queen];

            var occupancy = (friendlyOccupancy & ~blockersToRemove) | enemyOccupancy;

            while (pieces != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref pieces);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var horizontalPattern = GetHorizontalPattern(occupancy, piecePosition);
                var verticalPattern = GetVerticalPattern(occupancy, piecePosition);

                var pattern = (horizontalPattern | verticalPattern) & ~friendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    _bitBoard.Attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }
        }

        ulong GetHorizontalPattern(ulong occupancy, Position piecePosition)
        {
            var offset = piecePosition.Y - 1;

            var pieceRank = (byte)(occupancy >> (offset * 8));
            var pattern = PredefinedMoves.Rook[pieceRank, 8 - piecePosition.X];

            return (ulong)pattern << (offset * 8);
        }

        ulong GetVerticalPattern(ulong occupancy, Position piecePosition)
        {
            var offset = 8 - piecePosition.X;
            var rotatedOccupancy = BitOperations.RotateRight(occupancy);

            var pieceRank = (byte)(rotatedOccupancy >> (offset * 8));
            var pattern = PredefinedMoves.Rook[pieceRank, 8 - piecePosition.Y];
            
            return BitOperations.RotateLeft(pattern) << offset;
        }
    }
}
