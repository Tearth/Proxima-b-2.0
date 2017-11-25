using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Evaluation.PawnStructure.Chain
{
    public class PawnChainCalculator
    {
        public int GetChainValue(Color color, EvaluationParameters parameters)
        {
            var chain = 0;

            var pawns = parameters.Pieces[FastArray.GetPieceIndex(color, PieceType.Pawn)];
            var pawnsToParse = pawns;

            while (pawnsToParse != 0)
            {
                var pawnLSB = BitOperations.GetLSB(pawnsToParse);
                pawnsToParse = BitOperations.PopLSB(pawnsToParse);

                var chainMask = GetChainMask(color, pawnLSB);

                chain += BitOperations.Count(pawns & chainMask);
            }

            return chain * PawnStructureValues.PawnChainRatio[(int)parameters.GamePhase];
        }

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
