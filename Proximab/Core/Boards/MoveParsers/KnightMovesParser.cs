using Core.Boards.MoveGenerators;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using System.Collections.Generic;

namespace Core.Boards.MoveParsers
{
    public class KnightMovesParser
    {
        public KnightMovesParser()
        {

        }

        public List<Move> GetMoves(BitBoard bitBoard, Color color)
        {
            var moves = new List<Move>();
            var pieces = 0ul;

            var friendlyOccupation = bitBoard.Occupancy[(int)color - 1];
            var enemyOccupation = bitBoard.Occupancy[(int)ColorOperations.Invert(color) - 1];

            if (color == Color.White)
            {
                pieces = bitBoard.Pieces[(int)PieceType.WhiteKnight - 1];
            }
            else
            {
                pieces = bitBoard.Pieces[(int)PieceType.BlackKnight - 1];
            }
            
            while(pieces != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref pieces);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var pattern = PredefinedMoves.Knight[pieceIndex];

                while(pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);

                    var move = new Move(from, to, MoveType.None);
                    moves.Add(move);
                }
            }

            return moves;
        }
    }
}
