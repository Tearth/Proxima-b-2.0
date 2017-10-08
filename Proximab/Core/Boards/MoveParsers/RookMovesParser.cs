using Core.Boards.MoveGenerators;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using System.Collections.Generic;

namespace Core.Boards.MoveParsers
{
    class RookMovesParser
    {
        BitBoard _bitBoard;

        public RookMovesParser(BitBoard bitBoard)
        {
            _bitBoard = bitBoard;
        }

        public List<Move> GetMoves(Color color)
        {
            var moves = new List<Move>();

            var friendlyOccupancy = _bitBoard.Occupancy[(int)color];
            var enemyOccupancy = _bitBoard.Occupancy[(int)ColorOperations.Invert(color)];
            var occupancy = friendlyOccupancy | enemyOccupancy;

            var pieces = _bitBoard.Pieces[(int)color, (int)PieceType.Rook];

            while (pieces != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref pieces);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceLSB);

                var horizontalPattern = GetHorizontalPattern(occupancy, piecePosition);
                var verticalPattern = GetVerticalPattern(occupancy, piecePosition);

                var pattern = horizontalPattern | verticalPattern;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, enemyOccupancy);

                    var move = new Move(from, to, PieceType.Rook, color, moveType);
                    moves.Add(move);

                    _bitBoard.Attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }

            return moves;
        }

        ulong GetHorizontalPattern(ulong occupancy, Position piecePosition)
        {
            var offset = piecePosition.Y - 1;

            var pieceRankHorizontal = (byte)(occupancy >> (offset * 8));
            var pattern = PredefinedMoves.Rook[pieceRankHorizontal, 8 - piecePosition.X];

            return (ulong)pattern << (offset * 8);
        }

        ulong GetVerticalPattern(ulong occupancy, Position piecePosition)
        {
            var offset = 8 - piecePosition.X;
            var rotatedOccupancy = BitOperations.RotateRight(occupancy);

            var pieceRankVertical = (byte)(rotatedOccupancy >> (offset * 8));
            var pattern = PredefinedMoves.Rook[pieceRankVertical, 8 - piecePosition.Y];
            
            return BitOperations.RotateLeft(pattern) << offset;
        }
        
        MoveType GetMoveType(ulong patternLSB, ulong enemyOccupation)
        {
            if ((patternLSB & enemyOccupation) != 0)
            {
                return MoveType.Kill;
            }

            return MoveType.Quiet;
        }
    }
}
