using Proxima.Core.Boards;
using Proxima.Core.Commons;
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

        public static int GetEvaluation(BitBoard bitBoard, IncrementalEvaluationData incrementalEvaluationData)
        {
            // Temporary
            var gamePhase = GamePhase.Regular;

            var material = incrementalEvaluationData.Material;
            var mobility = _mobility.Calculate(gamePhase, bitBoard);
            var castling = incrementalEvaluationData.Castling;
            var position = incrementalEvaluationData.Position;
            var pawnStructure = _pawnStructure.Calculate(gamePhase, bitBoard);
            var kingSafety = _kingSafety.Calculate(gamePhase, bitBoard);

            return material + mobility + castling + position + pawnStructure + kingSafety;
        }

        public static DetailedEvaluationData GetDetailedEvaluation(BitBoard bitBoard)
        {
            // Temporary
            var gamePhase = GamePhase.Regular;

            return new DetailedEvaluationData()
            {
                Material = _material.CalculateDetailed(gamePhase, bitBoard),
                Mobility = _mobility.CalculateDetailed(gamePhase, bitBoard),
                Castling = _castling.CalculateDetailed(gamePhase, bitBoard),
                Position = _position.CalculateDetailed(gamePhase, bitBoard),
                PawnStructure = _pawnStructure.CalculateDetailed(gamePhase, bitBoard),
                KingSafety = _kingSafety.CalculateDetailed(gamePhase, bitBoard)
            };
        }
    }
}
