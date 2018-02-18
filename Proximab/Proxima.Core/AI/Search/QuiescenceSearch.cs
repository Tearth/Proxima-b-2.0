#define QUIESCENCE_SORT_MOVES

using System.Collections.Generic;
using System.Linq;
using Proxima.Core.AI.SEE;
using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.Search
{
    /// <summary>
    /// Represents a set of methods to do a quiescence search (only kill moves to avoid horizon effect).
    /// </summary>
    public class QuiescenceSearch : SearchBase
    {
        /// <summary>
        /// Starts a quiescence research.
        /// </summary>
        /// <param name="color">The current color.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="alpha">The alpha value.</param>
        /// <param name="beta">The beta value.</param>
        /// <param name="stats">The research statistics.</param>
        /// <returns>The value of current position after quiescence search.</returns>
        public int Do(Color color, Bitboard bitboard, int alpha, int beta, AIStats stats)
        {
            var enemyColor = ColorOperations.Invert(color);
            var colorSign = ColorOperations.ToSign(color);

            stats.QuiescenceTotalNodes++;

            var whiteGeneratorMode = GetGeneratorMode(color, Color.White);
            var blackGeneratorMode = GetGeneratorMode(color, Color.Black);
            bitboard.Calculate(whiteGeneratorMode, blackGeneratorMode, true);

            if (bitboard.IsCheck(enemyColor))
            {
                stats.QuiescenceEndNodes++;
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

            var sortedMoves = SortMoves(color, bitboard, bitboard.Moves);
            foreach (var move in sortedMoves)
            {
                var bitboardAfterMove = bitboard.Move(move);
                var nodeValue = -Do(enemyColor, bitboardAfterMove, -beta, -alpha, stats);

                if (nodeValue >= beta)
                {
                    return beta;
                }

                if (nodeValue > alpha)
                {
                    alpha = nodeValue;
                }
            }

            if (!sortedMoves.Any())
            {
                stats.QuiescenceEndNodes++;
            }

            return alpha;
        }

        /// <summary>
        /// Sorts the specified list of moves (best moves are higher which can cause more prunes).
        /// </summary>
        /// <param name="color">The current color.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="moves">The list of moves to sort.</param>
        /// <returns>The sorted list of moves.</returns>
        private List<Move> SortMoves(Color color, Bitboard bitboard, LinkedList<Move> moves)
        {
#if QUIESCENCE_SORT_MOVES
            var see = new SEECalculator();
            var seeResults = see.Calculate(color, bitboard);

            var sortedMoves = moves
                .Select(p =>
                    new
                    {
                        Move = p,
                        SEEScore = seeResults.FirstOrDefault(
                            q => q.InitialAttackerFrom == p.From &&
                                 q.InitialAttackerTo == p.To)?.Score ?? 100000
                    })
                .Where(p => p.SEEScore >= 0)
                .OrderByDescending(p => p.SEEScore)
                .Select(p => p.Move)
                .ToList();

            return sortedMoves;
#else
            return moves.ToList();
#endif
        }
    }
}
