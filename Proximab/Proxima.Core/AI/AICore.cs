using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Proxima.Core.AI.SEE;
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
            _transpositionTable.Clear();

            stopwatch.Start();
            do
            {
                result.Depth++;

                var stats = new AIStats();
                result.Score = colorSign * NegaMax(color, new Bitboard(bitboard), result.Depth, AIConstants.InitialAlphaValue, AIConstants.InitialBetaValue, stats);

                result.PVNodes = GetPVNodes(bitboard, color);
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
        public int NegaMax(Color color, Bitboard bitboard, int depth, int alpha, int beta, AIStats stats)
        {
            var bestValue = AIConstants.InitialAlphaValue;
            var colorSign = ColorOperations.ToSign(color);
            var enemyColor = ColorOperations.Invert(color);
            var boardHash = bitboard.GetHashForColor(color);
            var originalAlpha = alpha;

            stats.TotalNodes++;

            if (_transpositionTable.Exists(boardHash))
            {
                var transpositionNode = _transpositionTable.Get(boardHash);

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

                    if (alpha >= beta)
                    {
                        return transpositionNode.Score;
                    }
                }
            }

            if (depth <= 0)
            {
                stats.EndNodes++;
                return Quiescence(color, bitboard, alpha, beta, stats);
            }

            var whiteGeneratorMode = GetGeneratorMode(color, Color.White);
            var blackGeneratorMode = GetGeneratorMode(color, Color.Black);
            bitboard.Calculate(whiteGeneratorMode, blackGeneratorMode, false);

            if (bitboard.IsCheck(enemyColor))
            {
                stats.EndNodes++;
                return AIConstants.MateValue + depth;
            }

            Move bestMove = null;

            var availableMoves = SortMoves(color, bitboard, bitboard.Moves);
            foreach (var move in availableMoves)
            {
                var bitboardAfterMove = bitboard.Move(move);
                var nodeValue = -NegaMax(enemyColor, bitboardAfterMove, depth - 1, -beta, -alpha, stats);

                if (nodeValue > bestValue)
                {
                    bestValue = nodeValue;
                    bestMove = move;
                }

                alpha = Math.Max(nodeValue, alpha);

                if (alpha >= beta)
                {
                    stats.AlphaBetaCutoffs++;
                    break;
                }
            }

            var updateTranspositionNode = new TranspositionNode();
            updateTranspositionNode.Score = bestValue;
            updateTranspositionNode.Depth = depth;
            updateTranspositionNode.BestMove = bestMove;

            if (bestValue <= originalAlpha)
            {
                updateTranspositionNode.Type = ScoreType.UpperBound;
            }
            else if (bestValue >= beta)
            {
                updateTranspositionNode.Type = ScoreType.LowerBound;
            }
            else
            {
                updateTranspositionNode.Type = ScoreType.Exact;
            }

            _transpositionTable.AddOrUpdate(boardHash, updateTranspositionNode);

            return bestValue;
        }

        public int Quiescence(Color color, Bitboard bitboard, int alpha, int beta, AIStats stats)
        {
            var enemyColor = ColorOperations.Invert(color);
            var colorSign = ColorOperations.ToSign(color);

            stats.QuiescenceTotalNodes++;

            var whiteGeneratorMode = GetGeneratorMode(color, Color.White);
            var blackGeneratorMode = GetGeneratorMode(color, Color.Black);
            bitboard.Calculate(whiteGeneratorMode, blackGeneratorMode, true);

            if (bitboard.IsCheck(enemyColor))
            {
                return AIConstants.MateValue;
            }

            var evaluation = colorSign * bitboard.GetEvaluation();
            if (evaluation >= beta)
            {
                stats.QuiescenceEndNodes++;
                return beta;
            }

            if (evaluation > alpha)
            {
                alpha = evaluation;
            }

            var sortedMoves = SortQuiescenceMoves(color, bitboard, bitboard.Moves);
            foreach (var move in sortedMoves)
            {
                var bitboardAfterMove = bitboard.Move(move);
                var nodeValue = -Quiescence(enemyColor, bitboardAfterMove, -beta, -alpha, stats);

                if (nodeValue >= beta)
                {
                    stats.QuiescenceEndNodes++;
                    return beta;
                }

                if (nodeValue > alpha)
                {
                    alpha = nodeValue;
                }
            }

            stats.QuiescenceEndNodes++;
            return alpha;
        }

        private LinkedList<Move> SortMoves(Color color, Bitboard bitboard, LinkedList<Move> moves)
        {
            var sortedMoves = moves;

            var see = new SEECalculator();
            var seeResults = see.Calculate(color, bitboard);

            var boardHash = bitboard.GetHashForColor(color);
            if (_transpositionTable.Exists(boardHash))
            {
                var transpositionNode = _transpositionTable.Get(boardHash);
                if (transpositionNode.BestMove != null)
                {
                    var pvMove = moves.First(p =>
                        p.From == transpositionNode.BestMove.From && p.To == transpositionNode.BestMove.To);

                    sortedMoves.Remove(pvMove);
                    sortedMoves.AddFirst(pvMove);
                }
            }

            return sortedMoves;
        }

        private List<Move> SortQuiescenceMoves(Color color, Bitboard bitboard, LinkedList<Move> moves)
        {
            var see = new SEECalculator();
            var seeResults = see.Calculate(color, bitboard);

            var sortedMoves = moves
                .Select(p =>
                    new
                    {
                        Move = p,
                        SEEScore =  seeResults.FirstOrDefault(q => q.InitialAttackerFrom == p.From &&
                                                                   q.InitialAttackerTo == p.To)?.Score ?? 100000
                    })
                .Where(p => p.SEEScore >= 0)
                .OrderByDescending(p => p.SEEScore)
                .Select(p => p.Move)
                .ToList();

            return sortedMoves;
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

        private List<Move> GetPVNodes(Bitboard bitboard, Color color)
        {
            var pvNodes = new List<Move>();
            var boardHash = bitboard.GetHashForColor(color);

            while (_transpositionTable.Exists(boardHash) && pvNodes.Count < 20)
            {
                var pvNode = _transpositionTable.Get(boardHash);
                pvNodes.Add(pvNode.BestMove);

                bitboard = bitboard.Move(pvNode.BestMove);

                color = ColorOperations.Invert(color);
                boardHash = bitboard.GetHashForColor(color);
            }

            return pvNodes;
        }
    }
}
