﻿using Proxima.Core.Boards.PatternGenerators;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using System.Collections.Generic;

namespace Proxima.Core.Boards.MoveGenerators
{
    public class KnightMovesGenerator : MovesParserBase
    {
        public KnightMovesGenerator()
        {

        }

        public void Calculate(GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Knight)];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var pattern = PatternsContainer.KnightPattern[pieceIndex];

                while(pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    if ((opt.Mode & GeneratorMode.CalculateMoves) != 0 &&
                        (patternLSB & opt.FriendlyOccupancy) == 0)
                    {
                        var from = BitPositionConverter.ToPosition(pieceIndex);
                        var to = BitPositionConverter.ToPosition(patternIndex);

                        if ((patternLSB & opt.EnemyOccupancy) == 0)
                        {
                            opt.Moves.AddLast(new QuietMove(from, to, PieceType.Knight, opt.FriendlyColor));
                        }
                        else
                        {
                            opt.Moves.AddLast(new KillMove(from, to, PieceType.Knight, opt.FriendlyColor));
                        }
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
