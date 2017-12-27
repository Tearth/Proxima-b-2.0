using System;
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
        /// <summary>
        /// The event triggered when there is new thinking output available.
        /// </summary>
        public event EventHandler<ThinkingOutputEventArgs> OnThinkingOutput;

        /// <summary>
        /// Calculates the best possible move for the specified parameters.
        /// </summary>
        /// <param name="color">The initial player.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="preferredTime">Time allocated for AI.</param>
        /// <returns>The result of AI calculating.</returns>
        public AIResult Calculate(Color color, Bitboard bitboard, float preferredTime)
        {
            var result = new AIResult();
            var colorSign = ColorOperations.ToSign(color);
            var stopwatch = new Stopwatch();
            int estimatedTimeForNextIteration;

            result.PreferredTime = preferredTime;

            stopwatch.Start();
            do
            {
                result.Depth++;

                var stats = new AIStats();
                result.Score = colorSign * NegaMax(color, new Bitboard(bitboard), result.Depth, out Move bestMove, stats);

                result.BestMove = bestMove;
                result.Stats = stats;
                result.Ticks = stopwatch.Elapsed.Ticks;

                OnThinkingOutput?.Invoke(this, new ThinkingOutputEventArgs(result));

                estimatedTimeForNextIteration = (int)stopwatch.Elapsed.TotalMilliseconds * result.Stats.BranchingFactor;
            }
            while (estimatedTimeForNextIteration < preferredTime * 1000);
            
            return result;
        }

        /// <summary>
        /// Temporary method to calculating best move.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="depth">The current depth.</param>
        /// <param name="bestMove">The best possible move from nested nodes.</param>
        /// <param name="stats">The AI stats.</param>
        /// <returns>The evaluation score of best move.</returns>
        public int NegaMax(Color color, Bitboard bitboard, int depth, out Move bestMove, AIStats stats)
        {
            var bestValue = int.MinValue;
            var colorSign = ColorOperations.ToSign(color);
            var enemyColor = ColorOperations.Invert(color);
            bestMove = null;

            stats.TotalNodes++;

            if (depth <= 0)
            {
                bitboard.Calculate(GeneratorMode.CalculateAttacks, false);
                stats.EndNodes++;

                if (bitboard.IsCheck(enemyColor))
                {
                    return AIConstants.MateValue + depth;
                }

                return colorSign * bitboard.GetEvaluation();
            }
            else
            {
                var whiteGeneratorMode = GetGeneratorMode(color, Color.White);
                var blackGeneratorMode = GetGeneratorMode(color, Color.Black);
                bitboard.Calculate(whiteGeneratorMode, blackGeneratorMode, false);

                if (bitboard.IsCheck(enemyColor))
                {
                    stats.EndNodes++;
                    return AIConstants.MateValue + depth;
                }
            }

            var availableMoves = bitboard.Moves;
            foreach (var move in availableMoves)
            {
                var bitboardAfterMove = bitboard.Move(move);
                var nodeValue = -NegaMax(enemyColor, bitboardAfterMove, depth - 1, out _, stats);

                if (bestValue < nodeValue)
                {
                    bestValue = nodeValue;
                    bestMove = move;
                }
            }

            return bestValue;
        }

        /// <summary>
        /// Gets generator mode for the specified color.
        /// </summary>
        /// <param name="currentColor">The current color.</param>
        /// <param name="colorToMove">The color of moving player.</param>
        /// <returns>The generator mode.</returns>
        private GeneratorMode GetGeneratorMode(Color currentColor, Color colorToMove)
        {
            return currentColor == colorToMove ?
                GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks :
                GeneratorMode.CalculateAttacks;
        }
    }
}
