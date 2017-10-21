using Proxima.Core.Boards.PatternGenerators;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using System.Collections.Generic;

namespace Proxima.Core.Boards.MoveGenerators
{
    public class KnightMovesGenerator : MovesParserBase
    {
        public KnightMovesGenerator()
        {

        }

        public void GetMoves(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[(int)opt.FriendlyColor, (int)pieceType];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var pattern = PatternsContainer.KnightPattern[pieceIndex] & ~opt.FriendlyOccupancy;

                while(pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    if ((opt.Mode & GeneratorMode.CalculateMoves) != 0)
                    {
                        var from = BitPositionConverter.ToPosition(pieceIndex);
                        var to = BitPositionConverter.ToPosition(patternIndex);
                        var moveType = GetMoveType(patternLSB, opt.EnemyOccupancy);

                        opt.Moves.AddLast(new Move(from, to, pieceType, opt.FriendlyColor, moveType));
                    }

                    if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                    {
                        opt.Attacks[patternIndex] |= pieceLSB;
                        opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                    }
                }
            }
        }
    }
}
