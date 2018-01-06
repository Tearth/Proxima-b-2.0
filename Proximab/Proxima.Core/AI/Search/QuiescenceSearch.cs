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
    public class QuiescenceSearch : SearchBase
    {
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

        private List<Move> SortMoves(Color color, Bitboard bitboard, LinkedList<Move> moves)
        {
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
                .OrderByDescending(p => p.SEEScore)
                .Select(p => p.Move)
                .ToList();

            return sortedMoves;
        }
    }
}
