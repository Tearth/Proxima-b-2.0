using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.AI.SEE;
using Proxima.Core.AI.Transposition;
using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.Search
{
    public class RegularSearch : SearchBase
    {
        private TranspositionTable _transpositionTable;
        private QuiescenceSearch _quiescenceSearch;

        public RegularSearch(TranspositionTable transpositionTable)
        {
            _transpositionTable = transpositionTable;
            _quiescenceSearch = new QuiescenceSearch(_transpositionTable);
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
        public int Do(Color color, Bitboard bitboard, int depth, int alpha, int beta, AIStats stats)
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

            var availableMoves = SortMoves(color, bitboard, bitboard.Moves);
            foreach (var move in availableMoves)
            {
                var bitboardAfterMove = bitboard.Move(move);
                var nodeValue = -Do(enemyColor, bitboardAfterMove, depth - 1, -beta, -alpha, stats);

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
    }
}
