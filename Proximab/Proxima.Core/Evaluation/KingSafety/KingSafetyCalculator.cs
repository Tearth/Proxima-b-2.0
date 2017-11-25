using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.Evaluation.KingSafety
{
    public class KingSafetyCalculator
    {
        public int Calculate(EvaluationParameters parameters)
        {
            var whiteKingSafety = GetAttackedNeighboursValue(Color.White, parameters);
            var blackKingSafety = GetAttackedNeighboursValue(Color.Black, parameters);

            return whiteKingSafety - blackKingSafety;
        }

        public KingSafetyData CalculateDetailed(EvaluationParameters parameters)
        {
            return new KingSafetyData
            {
                WhiteAttackedNeighbours = GetAttackedNeighboursValue(Color.White, parameters),
                BlackAttackedNeighbours = GetAttackedNeighboursValue(Color.Black, parameters)
            };
        }

        private int GetAttackedNeighboursValue(Color color, EvaluationParameters parameters)
        {
            var attackedNeightbours = 0;

            var king = parameters.Pieces[FastArray.GetPieceIndex(color, PieceType.King)];
            var kingIndex = BitOperations.GetBitIndex(king);
            var kingMoves = PatternsContainer.KingPattern[kingIndex];

            while (kingMoves != 0)
            {
                var fieldLSB = BitOperations.GetLSB(kingMoves);
                kingMoves = BitOperations.PopLSB(kingMoves);

                var fieldIndex = BitOperations.GetBitIndex(fieldLSB);
                var attacks = parameters.Attacks[fieldIndex] & ~parameters.Occupancy[(int)color];

                attackedNeightbours += BitOperations.Count(attacks);
            }

            return attackedNeightbours * KingSafetyValues.AttackedNeighboursRatio[(int)parameters.GamePhase];
        }
    }
}
