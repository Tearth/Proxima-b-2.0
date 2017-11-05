using Proxima.Core.Evaluation.Castling;
using Proxima.Core.Evaluation.KingSafety;
using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Mobility;
using Proxima.Core.Evaluation.PawnStructure;
using Proxima.Core.Evaluation.Position;

namespace Proxima.Core.Evaluation
{
    public class EvaluationCalculator
    {
        public EvaluationResult GetEvaluation(EvaluationParameters parameters)
        {
            return new EvaluationResult()
            {
                Material = new MaterialCalculator().Calculate(parameters),
                Mobility = new MobilityCalculator().Calculate(parameters),
                Castling = new CastlingCalculator().Calculate(parameters),
                Position = new PositionCalculator().Calculate(parameters),
                PawnStructure = new PawnStructureCalculator().Calculate(parameters),
                KingSafety = new KingSafetyCalculator().Calculate(parameters)
            };
        }
    }
}
