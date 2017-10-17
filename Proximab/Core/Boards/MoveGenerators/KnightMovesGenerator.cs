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

        public void GetMoves(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[(int)opt.Color, (int)pieceType];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var pattern = PatternsContainer.KnightPattern[pieceIndex] & ~opt.FriendlyOccupancy;

                while(pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    if (opt.Mode == GeneratorMode.CalculateAll)
                    {
                        var from = BitPositionConverter.ToPosition(pieceIndex);
                        var to = BitPositionConverter.ToPosition(patternIndex);
                        var moveType = GetMoveType(patternLSB, opt.EnemyOccupancy);

                        opt.Moves.AddLast(new Move(from, to, pieceType, opt.Color, moveType));
                    }

                    if (opt.Mode == GeneratorMode.CalculateAll || opt.Mode == GeneratorMode.CalculateAttackFields)
                    {
                        opt.Attacks[(int)opt.Color, patternIndex] |= pieceLSB;
                    }
                }
            }
        }
    }
}
