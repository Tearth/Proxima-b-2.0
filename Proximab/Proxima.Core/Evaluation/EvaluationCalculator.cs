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
    /// <summary>
    /// Represents a set of methods to evaluation calculating.
    /// </summary>
    public static class EvaluationCalculator
    {
        private static MaterialCalculator _material = new MaterialCalculator();
        private static MobilityCalculator _mobility = new MobilityCalculator();
        private static CastlingCalculator _castling = new CastlingCalculator();
        private static PositionCalculator _position = new PositionCalculator();
        private static PawnStructureCalculator _pawnStructure = new PawnStructureCalculator();
        private static KingSafetyCalculator _kingSafety = new KingSafetyCalculator();

        /// <summary>
        /// Evaluates the specified bitboard (without any details, returns just simple number).
        /// </summary>
        /// <param name="bitboard">The bitboard to evaluate.</param>
        /// <returns>The bitboard evaluation result.</returns>
        public static int GetEvaluation(Bitboard bitboard)
        {
            var material = bitboard.IncEvaluation.Material;
            var mobility = _mobility.Calculate(bitboard);
            var castling = bitboard.IncEvaluation.Castling;
            var position = bitboard.IncEvaluation.Position;
            var pawnStructure = _pawnStructure.Calculate(bitboard);
            var kingSafety = _kingSafety.Calculate(bitboard);

            return material + mobility + castling + position + pawnStructure + kingSafety;
        }

        /// <summary>
        /// Evaluates the specified bitboard with details (only for debugging, too slow for AI).
        /// </summary>
        /// <param name="bitboard">The bitboard to evaluate.</param>
        /// <returns>The detailed bitboard evaluation result.</returns>
        public static DetailedEvaluationData GetDetailedEvaluation(Bitboard bitboard)
        {
            return new DetailedEvaluationData()
            {
                Material = _material.CalculateDetailed(bitboard),
                Mobility = _mobility.CalculateDetailed(bitboard),
                Castling = _castling.CalculateDetailed(bitboard),
                Position = _position.CalculateDetailed(bitboard),
                PawnStructure = _pawnStructure.CalculateDetailed(bitboard),
                KingSafety = _kingSafety.CalculateDetailed(bitboard)
            };
        }
    }
}
