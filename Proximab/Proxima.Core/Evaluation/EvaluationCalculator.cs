using Proxima.Core.Evaluation.Castling;
using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Mobility;
using Proxima.Core.Evaluation.Position;

namespace Proxima.Core.Evaluation
{
    public class EvaluationCalculator
    {
        MaterialCalculator _materialCalculator;
        MobilityCalculator _mobilityCalculator;
        CastlingCalculator _castlingCalculator;
        PositionCalculator _positionCalculator;

        public EvaluationCalculator()
        {
            _materialCalculator = new MaterialCalculator();
            _mobilityCalculator = new MobilityCalculator();
            _castlingCalculator = new CastlingCalculator();
            _positionCalculator = new PositionCalculator();
        }

        public EvaluationResult GetEvaluation(EvaluationParameters parameters)
        {
            return new EvaluationResult()
            {
                Material = _materialCalculator.Calculate(parameters),
                Mobility = _mobilityCalculator.Calculate(parameters),
                Castling = _castlingCalculator.Calculate(parameters),
                Position = _positionCalculator.Calculate(parameters)
            };
        }
    }
}
