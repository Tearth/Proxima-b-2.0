using Proxima.Core.Evaluation.Castling;
using Proxima.Core.Evaluation.KingSafety;
using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Mobility;
using Proxima.Core.Evaluation.PawnStructure;
using Proxima.Core.Evaluation.Position;

namespace Proxima.Core.Evaluation
{
    public static class EvaluationCalculator
    {
        static MaterialCalculator Material = new MaterialCalculator();
        static MobilityCalculator Mobility = new MobilityCalculator();
        static CastlingCalculator Castling = new CastlingCalculator();
        static PositionCalculator Position = new PositionCalculator();
        static PawnStructureCalculator PawnStructure = new PawnStructureCalculator();
        static KingSafetyCalculator KingSafety = new KingSafetyCalculator();

        public static EvaluationResult GetEvaluation(EvaluationParameters parameters)
        {
            return new EvaluationResult()
            {
                Material = Material.Calculate(parameters),
                Mobility = Mobility.Calculate(parameters),
                Castling = Castling.Calculate(parameters),
                Position = Position.Calculate(parameters),
                PawnStructure = PawnStructure.Calculate(parameters),
                KingSafety = KingSafety.Calculate(parameters)
            };
        }
    }
}
