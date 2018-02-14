using System;
using System.Collections.Generic;
using System.Linq;
using Proxima.Core.AI.HistoryHeuristic;
using Proxima.Core.AI.KillerHeuristic;
using Proxima.Core.AI.Patterns;
using Proxima.Core.AI.SEE;
using Proxima.Core.AI.Transposition;
using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.Search
{
    /// <summary>
    /// Represents a set of methods to do a regular search (full nodes search).
    /// </summary>
    public class RegularSearch : SearchBase
    {
        private TranspositionTable _transpositionTable;
        private HistoryTable _historyTable;
        private KillerTable _killerTable;
        private QuiescenceSearch _quiescenceSearch;
        private PatternsDetector _patternsDetector;

        Random randomNoise = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegularSearch"/> class.
        /// </summary>
        /// <param name="transpositionTable">The transposition table.</param>
        /// <param name="historyTable">The history table.</param>
        /// <param name="killerTable">The killer table.</param>
        public RegularSearch(TranspositionTable transpositionTable, HistoryTable historyTable, KillerTable killerTable)
        {
            _transpositionTable = transpositionTable;
            _historyTable = historyTable;
            _killerTable = killerTable;
            _quiescenceSearch = new QuiescenceSearch();
            _patternsDetector = new PatternsDetector();
        }

        /// <summary>
        /// Regular search, the core of AI algorithms.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="depth">The current depth.</param>
        /// <param name="alpha">The alpha value.</param>
        /// <param name="beta">The beta value.</param>
        /// <param name="deadline">The deadline (time after which search is immediately terminated).</param>
        /// <param name="helper">The flag indicating whether the search is an helper or not.</param>
        /// <param name="stats">The AI stats.</param>
        /// <returns>The evaluation score of best move.</returns>
        public int Do(Color color, Bitboard bitboard, int depth, int alpha, int beta, long deadline, bool helper, AIStats stats)
        {
            var root = stats.TotalNodes == 0;

            var bestValue = AIConstants.InitialAlphaValue;
            var enemyColor = ColorOperations.Invert(color);
            var boardHash = bitboard.GetHashForColor(color);
            var originalAlpha = alpha;

            stats.TotalNodes++;

            if (bitboard.IsThreefoldRepetition())
            {
                stats.EndNodes++;
                return 0;
            }

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
                return _quiescenceSearch.Do(color, bitboard, alpha, beta, stats);
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

            var availableMoves = SortMoves(color, depth, bitboard, bitboard.Moves, helper);
            var firstMove = true;

            foreach (var move in availableMoves)
            {
                if (DateTime.Now.Ticks >= deadline)
                {
                    break;
                }

                if (root)
                {
                    if (_patternsDetector.IsPattern(bitboard, move))
                    {
                        continue;
                    }
                }

                var bitboardAfterMove = bitboard.Move(move);
                var nodeValue = 0;

                if (firstMove)
                {
                    nodeValue = -Do(enemyColor, bitboardAfterMove, depth - 1, -beta, -alpha, deadline, helper, stats);
                    firstMove = false;
                }
                else
                {
                    nodeValue = -Do(enemyColor, bitboardAfterMove, depth - 1, -alpha - 1, -alpha, deadline, helper, stats);

                    if (nodeValue > alpha && nodeValue < beta)
                    {
                        bitboardAfterMove = bitboard.Move(move);
                        nodeValue = -Do(enemyColor, bitboardAfterMove, depth - 1, -beta, -alpha, deadline, helper, stats);
                    }
                }

                if (nodeValue > bestValue)
                {
                    bestValue = nodeValue;
                    bestMove = move;
                }

                alpha = Math.Max(nodeValue, alpha);

                if (alpha >= beta)
                {
                    if (move is QuietMove)
                    {
                        _historyTable.AddKiller(color, depth, bestMove);
                        _killerTable.AddKiller(depth, move);
                    }

                    stats.AlphaBetaCutoffs++;

                    break;
                }
            }

            if (bestValue == -(AIConstants.MateValue + depth - 1) && !bitboard.IsCheck(color))
            {
                stats.EndNodes++;
                return 0;
            }

            var updateTranspositionNode = new TranspositionNode
            {
                Score = bestValue,
                Depth = depth,
                BestMove = bestMove,
                Type = GetTranspositionNodeType(originalAlpha, beta, bestValue)
            };

            _transpositionTable.AddOrUpdate(boardHash, updateTranspositionNode);

            return bestValue;
        }

        /// <summary>
        /// Gets a transposition node type based on alpha, beta and best move values.
        /// </summary>
        /// <param name="alpha">The alpha value.</param>
        /// <param name="beta">The beta value.</param>
        /// <param name="bestValue">The best move value.</param>
        /// <returns>The transposition node type.</returns>
        private ScoreType GetTranspositionNodeType(int alpha, int beta, int bestValue)
        {
            return bestValue <= alpha ? ScoreType.UpperBound :
                   bestValue >= beta ? ScoreType.LowerBound :
                   ScoreType.Exact;
        }

        /// <summary>
        /// Sorts the specified list of moves (best moves are higher which can cause more prunes).
        /// </summary>
        /// <param name="color">The current color.</param>
        /// <param name="depth">The current depth.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="moves">The list of moves to sort.</param>
        /// <param name="helper">The flag indicating whether the search is an helper or not.</param>
        /// <returns>The sorted list of moves.</returns>
        private List<Move> SortMoves(Color color, int depth, Bitboard bitboard, LinkedList<Move> moves, bool helper)
        {
            var sortedMoves = moves.Select(p => new RegularSortedMove { Move = p, Score = -100000 }).ToList();

            AssignSpecialScores(sortedMoves, depth);
            AssignSEEScores(color, bitboard, sortedMoves);
            AssignHashScore(color, bitboard, sortedMoves);

            if (helper)
            {
                foreach (var move in sortedMoves)
                {
                    move.Score += randomNoise.Next(-AIConstants.RegularSearchNoiseForHelpers, AIConstants.RegularSearchNoiseForHelpers);
                }
            }

            return sortedMoves.OrderByDescending(p => p.Score).Select(p => p.Move).ToList();
        }

        /// <summary>
        /// Assigns score to move which was the best in the previous iteration. It has a big potential to cause beta cut-off, so
        /// should be searched as first.
        /// </summary>
        /// <param name="color">The current color.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="movesToSort">The list of moves to sort.</param>
        private void AssignHashScore(Color color, Bitboard bitboard, List<RegularSortedMove> movesToSort)
        {
            var boardHash = bitboard.GetHashForColor(color);
            if (_transpositionTable.Exists(boardHash))
            {
                var transpositionNode = _transpositionTable.Get(boardHash);
                if (transpositionNode.BestMove != null)
                {
                    var pvMove = movesToSort.First(p =>
                        p.Move.From == transpositionNode.BestMove.From && p.Move.To == transpositionNode.BestMove.To);

                    pvMove.Score = 100000;
                }
            }
        }

        /// <summary>
        /// Assigns scores based on Static Exchange Evaluation results. Kill moves that are winning are higher than losing moves.
        /// </summary>
        /// <param name="color">The current color.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="movesToSort">The list of moves to sort.</param>
        private void AssignSEEScores(Color color, Bitboard bitboard, List<RegularSortedMove> movesToSort)
        {
            var see = new SEECalculator();
            var seeResults = see.Calculate(color, bitboard);

            foreach (var seeResult in seeResults)
            {
                var sortedMove = movesToSort.FirstOrDefault(p => p.Move.From == seeResult.InitialAttackerFrom &&
                                                                 p.Move.To == seeResult.InitialAttackerTo);

                if (sortedMove != null)
                {
                    if (seeResult.Score < 0)
                    {
                        seeResult.Score *= 1000;
                    }

                    sortedMove.Score = seeResult.Score;
                }
            }
        }

        /// <summary>
        /// Assigns special scores (promotions, castling, killer and history table).
        /// </summary>
        /// <param name="movesToSort">The moves to sort.</param>
        /// <param name="depth">the current depth.</param>
        private void AssignSpecialScores(List<RegularSortedMove> movesToSort, int depth)
        {
            foreach (var move in movesToSort)
            {
                var killer = _historyTable.GetKillersCount(move.Move);
                if (move.Move is PromotionMove || move.Move is CastlingMove)
                {
                    move.Score = 50000;
                }
                else if (_killerTable.IsKiller(depth, move.Move))
                {
                    move.Score = 10;
                }
                else
                {
                    move.Score = -(1000 - killer);
                }
            }
        }
    }
}
