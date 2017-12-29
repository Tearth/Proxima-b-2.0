using System;
using System.Diagnostics;
using Proxima.Core.AI.Transposition;
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

        private TranspositionTable _transpositionTable;

        public AICore()
        {
            _transpositionTable = new TranspositionTable();
        }

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
                result.Score = colorSign * NegaMax(color, new Bitboard(bitboard), result.Depth, AIConstants.InitialAlphaValue, AIConstants.InitialBetaValue, out var bestMove, stats);

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
        public int NegaMax(Color color, Bitboard bitboard, int depth, int alpha, int beta, out Move bestMove, AIStats stats)
        {
            var bestValue = AIConstants.InitialAlphaValue;
            var colorSign = ColorOperations.ToSign(color);
            var enemyColor = ColorOperations.Invert(color);
            var originalAlpha = alpha;
            bestMove = null;

            stats.TotalNodes++;

            if (_transpositionTable.Exists(bitboard.Hash))
            {
                var transpositionNode = _transpositionTable.Get(bitboard.Hash);

                if (transpositionNode.Depth >= depth)
                {
                    stats.TranspositionTableHits++;
                    switch (transpositionNode.Type)
                    {
                        case ScoreType.Exact:
                        {
                            return transpositionNode.Score;
                        }

                        case ScoreType.LowerBound:
                        {
                            alpha = Math.Max(alpha, transpositionNode.Score);
                            break;
                        }

                        case ScoreType.UpperBound:
                        {
                            beta = Math.Min(beta, transpositionNode.Score);
                            break;
                        }
                    }
                }

                if (alpha >= beta)
                {
                    return transpositionNode.Score;
                }
            }

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

            var whiteGeneratorMode = GetGeneratorMode(color, Color.White);
            var blackGeneratorMode = GetGeneratorMode(color, Color.Black);
            bitboard.Calculate(whiteGeneratorMode, blackGeneratorMode, false);

            if (bitboard.IsCheck(enemyColor))
            {
                stats.EndNodes++;
                return AIConstants.MateValue + depth;
            }

            var availableMoves = bitboard.Moves;
            foreach (var move in availableMoves)
            {
                var bitboardAfterMove = bitboard.Move(move);
                var nodeValue = -NegaMax(enemyColor, bitboardAfterMove, depth - 1, -beta, -alpha, out _, stats);

                if (nodeValue > bestValue)
                {
                    bestValue = nodeValue;
                    bestMove = move;
                }

                if (nodeValue > alpha)
                {
                    alpha = nodeValue;
                }

                if (alpha >= beta)
                {
                    stats.AlphaBetaCutoffs++;
                    break;
                }
            }

            var updateDranspositionNode = new TranspositionNode();
            updateDranspositionNode.Score = bestValue;
            updateDranspositionNode.Depth = depth;

            if (bestValue <= originalAlpha)
            {
                updateDranspositionNode.Type = ScoreType.UpperBound;
            }
            else if (bestValue >= beta)
            {
                updateDranspositionNode.Type = ScoreType.LowerBound;
            }
            else
            {
                updateDranspositionNode.Type = ScoreType.Exact;
            }
            _transpositionTable.AddOrUpdate(bitboard.Hash, updateDranspositionNode);

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
