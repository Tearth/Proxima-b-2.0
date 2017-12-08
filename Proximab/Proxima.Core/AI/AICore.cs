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
        public int NegaMax(Color color, Bitboard bitboard, int depth, out Move bestMove, ref int totalNodes, ref int endNodes)
        {
            bestMove = null;

            totalNodes++;
            if (depth == 0)
            {
                endNodes++;
                return -(((int)color * 2) - 1) * bitboard.GetEvaluation();
            }


            var bestValue = int.MinValue;
            var enemyColor = ColorOperations.Invert(color);
            var whiteGeneratorMode = GetGeneratorMode(Color.White, color);
            var blackGeneratorMode = GetGeneratorMode(Color.Black, color);

            bitboard.Calculate(whiteGeneratorMode, blackGeneratorMode);
            var availableMoves = bitboard.Moves;

            foreach(var move in availableMoves)
            {
                var bitboardAfterMove = bitboard.Move(move);
                var nodeValue = -NegaMax(enemyColor, bitboardAfterMove, depth - 1, out _, ref totalNodes, ref endNodes);

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
