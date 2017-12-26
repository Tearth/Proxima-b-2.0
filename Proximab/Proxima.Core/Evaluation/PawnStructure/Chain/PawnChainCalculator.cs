using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Evaluation.PawnStructure.Chain
{
    /// <summary>
    /// Represents a set of methods to evaluate pawn chain.
    /// </summary>
    /// <remarks>
    /// Pawn chain is a structure consisting of two or more sticked pawns on the same diagonal. This is the
    /// preferred situation because these pawns are easier to defend and are great barrier against the
    /// enemy pieces.
    /// </remarks>
    public class PawnChainCalculator
    {
        /// <summary>
        /// Calculates a chain evaluation result for the specified player by adding number of pawns
        /// in chains multiplied by the specified ratio.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The player color.</param>
        /// <returns>The chain evaluation result for the specified player.</returns>
        public int GetChainValue(Bitboard bitboard, Color color)
        {
            var chain = 0;

            var pawns = bitboard.Pieces[FastArray.GetPieceIndex(color, PieceType.Pawn)];
            var pawnsToParse = pawns;

            while (pawnsToParse != 0)
            {
                var pawnLSB = BitOperations.GetLSB(pawnsToParse);
                pawnsToParse = BitOperations.PopLSB(pawnsToParse);

                var chainMask = GetChainMask(color, pawnLSB);

                chain += BitOperations.Count(pawns & chainMask);
            }

            return chain * PawnStructureValues.PawnChainRatio[(int)bitboard.GamePhase];
        }

        /// <summary>
        /// Calculates a chain mask for the specified color and field.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <param name="pawnLSB">The LSB of the specified field.</param>
        /// <returns>The chain mask for the specified color and field.</returns>
        private ulong GetChainMask(Color color, ulong pawnLSB)
        {
            var mask = ((pawnLSB & ~BitConstants.AFile) << 1) | ((pawnLSB & ~BitConstants.HFile) >> 1);

            if (color == Color.White)
            {
                mask = (mask & ~BitConstants.HRank) << 8;
            }
            else
            {
                mask = (mask & ~BitConstants.ARank) >> 8;
            }

            return mask;
        }
    }
}
