using System.Diagnostics;
using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI
{
    /// <summary>
    /// Core class of Proxima b AI.
    /// </summary>
    public class AICore
    {
        public AIResult Calculate(Color color, Bitboard bitboard, int depth)
        {
            var result = new AIResult();
            var stats = new AIStats();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            result.Score = NegaMax(color, bitboard, depth, out Move bestMove, stats);
            result.Ticks = stopwatch.Elapsed.Ticks;

            result.BestMove = bestMove;
            result.Stats = stats;

            return result;
        }

        public int NegaMax(Color color, Bitboard bitboard, int depth, out Move bestMove, AIStats stats)
        {
            var bestValue = int.MinValue;
            var enemyColor = ColorOperations.Invert(color);
            bestMove = null;

            stats.TotalNodes++;
            if (depth <= 0)
            {
                bitboard.Calculate(GeneratorMode.CalculateAttacks, GeneratorMode.CalculateAttacks);
                stats.EndNodes++;

                return -(((int)color * 2) - 1) * bitboard.GetEvaluation();
            }
            else
            {
                var whiteGeneratorMode = GetGeneratorMode(color, Color.White);
                var blackGeneratorMode = GetGeneratorMode(color, Color.Black);
                bitboard.Calculate(whiteGeneratorMode, blackGeneratorMode);
            }

            var availableMoves = bitboard.Moves;
            foreach(var move in availableMoves)
            {
                var bitboardAfterMove = bitboard.Move(move);
                var nodeValue = -NegaMax(enemyColor, bitboardAfterMove, depth - 1, out _, stats);

                if(bestValue < nodeValue)
                {
                    bestValue = nodeValue;
                    bestMove = move;
                }
            }

            return bestValue;
        }

        private GeneratorMode GetGeneratorMode(Color currentColor, Color colorToMove)
        {
            return currentColor == colorToMove  ?
                GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks :
                GeneratorMode.CalculateAttacks;
        }
    }
}
