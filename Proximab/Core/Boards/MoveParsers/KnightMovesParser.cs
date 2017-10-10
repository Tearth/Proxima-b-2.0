using Core.Boards.MoveGenerators;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using System.Collections.Generic;

namespace Core.Boards.MoveParsers
{
    public class KnightMovesParser : MovesParserBase
    {
        public KnightMovesParser()
        {

        }

        public List<Move> GetMoves(PieceType pieceType, Color color, ulong[,] pieces, ulong[] occupancy, ref ulong[,] attacks)
        {
            var moves = new List<Move>();

            var friendlyOccupation = occupancy[(int)color];
            var enemyOccupation = occupancy[(int)ColorOperations.Invert(color)];

            var piecesToParse = pieces[(int)color, (int)pieceType];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var pattern = PredefinedMoves.KnightMoves[pieceIndex] & ~friendlyOccupation;

                while(pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, enemyOccupation);
                    
                    moves.Add(new Move(from, to, pieceType, color, moveType));

                    attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }

            return moves;
        }
    }
}
