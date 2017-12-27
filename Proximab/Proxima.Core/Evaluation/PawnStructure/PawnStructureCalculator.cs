using Proxima.Core.Boards;
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
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The pawn structure evaluation result.</returns>
        public int Calculate(Bitboard bitboard)
        {
            var whiteDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(bitboard, Color.White);
            var blackDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(bitboard, Color.Black);

            var whiteIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(bitboard, Color.White);
            var blackIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(bitboard, Color.Black);

            var whitePawnChains = _pawnChainCalculator.GetChainValue(bitboard, Color.White);
            var blackPawnChains = _pawnChainCalculator.GetChainValue(bitboard, Color.Black);

            return whiteDoubledPawns - blackDoubledPawns +
                   (whiteIsolatedPawns - blackIsolatedPawns) +
                   (whitePawnChains - blackPawnChains);
        }

        /// <summary>
        /// Calculates a detailed pawn structure evaluation result.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The detailed pawn structure evaluation result.</returns>
        public PawnStructureData CalculateDetailed(Bitboard bitboard)
        {
            return new PawnStructureData()
            {
                WhiteDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(bitboard, Color.White),
                BlackDoubledPawns = _doubledPawnsCalculator.GetDoubledPawnsValue(bitboard, Color.Black),

                WhiteIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(bitboard, Color.White),
                BlackIsolatedPawns = _isolatedPawnsCalculator.GetIsolatedPawnsValue(bitboard, Color.Black),

                WhitePawnChain = _pawnChainCalculator.GetChainValue(bitboard, Color.White),
                BlackPawnChain = _pawnChainCalculator.GetChainValue(bitboard, Color.Black),
            };
        }
    }
}
