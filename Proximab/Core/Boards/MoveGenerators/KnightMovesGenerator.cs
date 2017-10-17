using Core.Boards.PatternGenerators;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using System.Collections.Generic;

namespace Core.Boards.MoveGenerators
{
    public class KnightMovesGenerator : MovesParserBase
    {
        public KnightMovesGenerator()
        {

        }

        public void GetMoves(PieceType pieceType, Color color, GeneratorMode mode, ulong[,] pieces, OccupancyContainer occupancyContainer, LinkedList<Move> moves, ulong[,] attacks)
        {
            var piecesToParse = pieces[(int)color, (int)pieceType];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var pattern = PatternsContainer.KnightPattern[pieceIndex] & ~occupancyContainer.FriendlyOccupancy;

                while(pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    if (mode == GeneratorMode.CalculateAll)
                    {
                        var from = BitPositionConverter.ToPosition(pieceIndex);
                        var to = BitPositionConverter.ToPosition(patternIndex);
                        var moveType = GetMoveType(patternLSB, occupancyContainer.EnemyOccupancy);

                        moves.AddLast(new Move(from, to, pieceType, color, moveType));
                    }

                    if (mode == GeneratorMode.CalculateAll || mode == GeneratorMode.CalculateAttackFields)
                    {
                        attacks[(int)color, patternIndex] |= pieceLSB;
                    }
                }
            }
        }
    }
}
