using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators
{
    /// <summary>
    /// Represents a set of methods to generating knight moves.
    /// </summary>
    public static class KnightMovesGenerator
    {
        /// <summary>
        /// Generates available moves.
        /// </summary>
        /// <param name="opt">The generator parameters.</param>
        public static void Generate(GeneratorParameters opt)
        {
            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Knight)];

            while (piecesToParse != 0)
            {
                var pieceLsb = BitOperations.GetLsb(piecesToParse);
                piecesToParse = BitOperations.PopLsb(piecesToParse);

                var pieceIndex = BitOperations.GetBitIndex(pieceLsb);
                var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

                var pattern = PatternsContainer.KnightPattern[pieceIndex];

                while (pattern != 0)
                {
                    var patternLsb = BitOperations.GetLsb(pattern);
                    pattern = BitOperations.PopLsb(pattern);

                    var patternIndex = BitOperations.GetBitIndex(patternLsb);

                    if ((opt.Mode & GeneratorMode.CalculateMoves) != 0 &&
                        (patternLsb & opt.FriendlyOccupancy) == 0)
                    {
                        var to = BitPositionConverter.ToPosition(patternIndex);

                        if ((patternLsb & opt.EnemyOccupancy) == 0 && !opt.QuiescenceSearch)
                        {
                            opt.Bitboard.Moves.AddLast(new QuietMove(piecePosition, to, PieceType.Knight, opt.FriendlyColor));
                        }
                        else if ((patternLsb & opt.EnemyOccupancy) != 0)
                        {
                            opt.Bitboard.Moves.AddLast(new KillMove(piecePosition, to, PieceType.Knight, opt.FriendlyColor));
                        }
                    }

                    if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                    {
                        opt.Bitboard.Attacks[patternIndex] |= pieceLsb;
                        opt.Bitboard.AttacksSummary[(int)opt.FriendlyColor] |= patternLsb;
                    }
                }
            }
        }
    }
}
