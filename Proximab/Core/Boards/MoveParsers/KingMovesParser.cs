using Core.Boards.MoveGenerators;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using System.Collections.Generic;

namespace Core.Boards.MoveParsers
{
    public class KingMovesParser : MovesParserBase
    {
        public KingMovesParser()
        {

        }

        public void GetMoves(PieceType pieceType, Color color, ulong[,] pieces, OccupancyContainer occupancyContainer, LinkedList<Move> moves, ref ulong[,] attacks)
        {
            var piecesToParse = pieces[(int)color, (int)pieceType];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var pattern = PredefinedMoves.KingMoves[pieceIndex] & ~occupancyContainer.FriendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, occupancyContainer.EnemyOccupancy);
                    
                    moves.AddLast(new Move(from, to, pieceType, color, moveType));

                    attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }
        }
    }
}
