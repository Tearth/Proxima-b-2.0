using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators
{
    public static class KnightMovesGenerator
    {
        public static void Calculate(GeneratorParameters opt)
        {
            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Knight)];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(piecesToParse);
                piecesToParse = BitOperations.PopLSB(piecesToParse);

                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

                var pattern = PatternsContainer.KnightPattern[pieceIndex];

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(pattern);
                    pattern = BitOperations.PopLSB(pattern);

                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    if ((opt.Mode & GeneratorMode.CalculateMoves) != 0 &&
                        (patternLSB & opt.FriendlyOccupancy) == 0)
                    {
                        var to = BitPositionConverter.ToPosition(patternIndex);

                        if ((patternLSB & opt.EnemyOccupancy) == 0)
                        {
                            opt.Bitboard.Moves.AddLast(new QuietMove(piecePosition, to, PieceType.Knight, opt.FriendlyColor));
                        }
                        else
                        {
                            opt.Bitboard.Moves.AddLast(new KillMove(piecePosition, to, PieceType.Knight, opt.FriendlyColor));
                        }
                    }

                    if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                    {
                        opt.Bitboard.Attacks[patternIndex] |= pieceLSB;
                        opt.Bitboard.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                    }
                }
            }
        }
    }
}
