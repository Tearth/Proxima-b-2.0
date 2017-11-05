using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.Evaluation.KingSafety
{
    public class KingSafetyCalculator
    {
        public KingSafetyResult Calculate(EvaluationParameters parameters)
        {
            return new KingSafetyResult
            {
                WhiteAttackedNeighbours = GetAttackedNeighbours(Color.White, parameters),
                BlackAttackedNeighbours = GetAttackedNeighbours(Color.Black, parameters)
            };
        }

        int GetAttackedNeighbours(Color color, EvaluationParameters parameters)
        {
            var attackedNeightbours = 0;

            var king = parameters.Pieces[FastArray.GetPieceIndex(color, PieceType.King)];
            var kingIndex = BitOperations.GetBitIndex(king);
            var kingMoves = PatternsContainer.KingPattern[kingIndex];
            
            while(kingMoves != 0)
            {
                var fieldLSB = BitOperations.GetLSB(ref kingMoves);
                var fieldIndex = BitOperations.GetBitIndex(fieldLSB);
                var attacks = parameters.Attacks[fieldIndex] & ~parameters.Occupancy[(int)color];

                attackedNeightbours += BitOperations.Count(attacks);
            }

            return attackedNeightbours * KingSafetyValues.AttackedNeighboursRatio[(int)parameters.GamePhase];
        }
    }
}
