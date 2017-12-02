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
        /// <param name="bitBoard">The bitboard to evaluate.</param>
        /// <returns>The bitboard evaluation result.</returns>
        public static int GetEvaluation(BitBoard bitBoard)
        {
            // Temporary
            var gamePhase = GamePhase.Regular;

            var material = bitBoard.IncEvaluation.Material;
            var mobility = _mobility.Calculate(gamePhase, bitBoard);
            var castling = bitBoard.IncEvaluation.Castling;
            var position = bitBoard.IncEvaluation.Position;
            var pawnStructure = _pawnStructure.Calculate(gamePhase, bitBoard);
            var kingSafety = _kingSafety.Calculate(gamePhase, bitBoard);

            return material + mobility + castling + position + pawnStructure + kingSafety;
        }

        /// <summary>
        /// Evaluates the specified bitboard with details (only for debugging, too slow for AI).
        /// </summary>
        /// <param name="bitBoard">The bitboard to evaluate.</param>
        /// <returns>The detailed bitboard evaluation result.</returns>
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
