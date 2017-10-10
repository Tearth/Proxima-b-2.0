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

                ulong pattern = GetRightRotatedBitBoardPattern(occupancy, pieceLSB);
                pattern |= GetLeftRotatedBitBoardPattern(occupancy, pieceLSB);

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
    }
}
