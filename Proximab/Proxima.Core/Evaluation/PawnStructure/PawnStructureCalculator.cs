using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Evaluation.PawnStructure.Chain;
using Proxima.Core.Evaluation.PawnStructure.Doubled;
using Proxima.Core.Evaluation.PawnStructure.Isolated;

namespace Proxima.Core.Evaluation.PawnStructure
{
    /// <summary>
    /// Represents a set of methods to evaluate pawn structure.
    /// </summary>
    public class PawnStructureCalculator
    {
        private DoubledPawnsCalculator _doubledPawnsCalculator;
        private IsolatedPawnsCalculator _isolatedPawnsCalculator;
        private PawnChainCalculator _pawnChainCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PawnStructureCalculator"/> class.
        /// </summary>
        public PawnStructureCalculator()
        {
            _doubledPawnsCalculator = new DoubledPawnsCalculator();
            _isolatedPawnsCalculator = new IsolatedPawnsCalculator();
            _pawnChainCalculator = new PawnChainCalculator();
        }

        /// <summary>
        /// Calculates a pawn structure evaluation result.
        /// </summary>
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="bitBoard">The bitboard.</param>
        /// <returns>The pawn structure evaluation result.</returns>
        public int Calculate(GamePhase gamePhase, BitBoard bitBoard)
        {
            var whiteDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(Color.White, gamePhase, bitBoard);
            var blackDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(Color.Black, gamePhase, bitBoard);

            var whiteIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(Color.White, gamePhase, bitBoard);
            var blackIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(Color.Black, gamePhase, bitBoard);

            var whitePawnChains = _pawnChainCalculator.GetChainValue(Color.White, gamePhase, bitBoard);
            var blackPawnChains = _pawnChainCalculator.GetChainValue(Color.Black, gamePhase, bitBoard);

            return (whiteDoubledPawns - blackDoubledPawns) +
                   (whiteIsolatedPawns - blackIsolatedPawns) +
                   (whitePawnChains - blackPawnChains);
        }

        /// <summary>
        /// Calculates a detailed pawn structure evaluation result.
        /// </summary>
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="bitBoard">The bitboard.</param>
        /// <returns>The detailed pawn structure evaluation result.</returns>
        public PawnStructureData CalculateDetailed(GamePhase gamePhase, BitBoard bitBoard)
        {
            return new PawnStructureData()
            {
                WhiteDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(Color.White, gamePhase, bitBoard),
                BlackDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(Color.Black, gamePhase, bitBoard),

                WhiteIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(Color.White, gamePhase, bitBoard),
                BlackIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(Color.Black, gamePhase, bitBoard),

                WhitePawnChain = _pawnChainCalculator.GetChainValue(Color.White, gamePhase, bitBoard),
                BlackPawnChain = _pawnChainCalculator.GetChainValue(Color.Black, gamePhase, bitBoard),
            };
        }
    }
}
