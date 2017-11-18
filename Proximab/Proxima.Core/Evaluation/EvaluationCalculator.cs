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

        public static int GetEvaluation(EvaluationParameters parameters, IncrementalEvaluationData incrementalEvaluationData)
        {
            var material = Material.Calculate(parameters);
            var mobility = Mobility.Calculate(parameters);
            var castling = Castling.Calculate(parameters);
            var position = Position.Calculate(parameters);
            var pawnStructure = PawnStructure.Calculate(parameters);
            var kingSafety = KingSafety.Calculate(parameters);

            return material + mobility + castling + position + pawnStructure + kingSafety;
        }

        public static DetailedEvaluationData GetDetailedEvaluation(EvaluationParameters parameters)
        {
            return new DetailedEvaluationData()
            {
                Material = Material.CalculateDetailed(parameters),
                Mobility = Mobility.CalculateDetailed(parameters),
                Castling = Castling.CalculateDetailed(parameters),
                Position = Position.CalculateDetailed(parameters),
                PawnStructure = PawnStructure.CalculateDetailed(parameters),
                KingSafety = KingSafety.CalculateDetailed(parameters)
            };
        }
    }
}
