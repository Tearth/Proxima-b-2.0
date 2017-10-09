using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
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

            CalculateMoves(pieces, color, friendlyOccupancy, enemyOccupancy);

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

                ulong pattern = 0xFF;

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
    }
}
