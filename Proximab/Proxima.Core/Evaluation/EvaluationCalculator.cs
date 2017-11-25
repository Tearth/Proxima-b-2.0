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
        private static MaterialCalculator _material = new MaterialCalculator();
        private static MobilityCalculator _mobility = new MobilityCalculator();
        private static CastlingCalculator _castling = new CastlingCalculator();
        private static PositionCalculator _position = new PositionCalculator();
        private static PawnStructureCalculator _pawnStructure = new PawnStructureCalculator();
        private static KingSafetyCalculator _kingSafety = new KingSafetyCalculator();

        public static int GetEvaluation(EvaluationParameters parameters, IncrementalEvaluationData incrementalEvaluationData)
        {
            var material = incrementalEvaluationData.Material;
            var mobility = _mobility.Calculate(parameters);
            var castling = incrementalEvaluationData.Castling;
            var position = incrementalEvaluationData.Position;
            var pawnStructure = _pawnStructure.Calculate(parameters);
            var kingSafety = _kingSafety.Calculate(parameters);

            return material + mobility + castling + position + pawnStructure + kingSafety;
        }

        public static DetailedEvaluationData GetDetailedEvaluation(EvaluationParameters parameters)
        {
            return new DetailedEvaluationData()
            {
                Material = _material.CalculateDetailed(parameters),
                Mobility = _mobility.CalculateDetailed(parameters),
                Castling = _castling.CalculateDetailed(parameters),
                Position = _position.CalculateDetailed(parameters),
                PawnStructure = _pawnStructure.CalculateDetailed(parameters),
                KingSafety = _kingSafety.CalculateDetailed(parameters)
            };
        }
    }
}
