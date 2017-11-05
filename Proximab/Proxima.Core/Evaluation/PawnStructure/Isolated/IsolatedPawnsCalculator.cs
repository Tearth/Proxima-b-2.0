using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using System.Linq;

namespace Proxima.Core.Evaluation.PawnStructure.Isolated
{
    public class IsolatedPawnsCalculator
    {
        public int GetIsolatedPawnsValue(Color color, EvaluationParameters parameters)
        {
            var isolatedPawns = 0;
            var pawns = parameters.Pieces[FastArray.GetPieceIndex(color, PieceType.Pawn)];

            for (int i = 0; i < 8; i++)
            {
                var file = BitConstants.HFile << i;

                var pawnsInFile = pawns & file;
                var previousLinePawns = pawns & ((file & ~BitConstants.AFile) << 1);
                var nextLinePawns = pawns & ((file & ~BitConstants.HFile) >> 1);

                if (pawnsInFile != 0 && previousLinePawns == 0 && nextLinePawns == 0)
                {
                    isolatedPawns += BitOperations.Count(pawnsInFile);
                }
            }

            return isolatedPawns * PawnStructureValues.IsolatededPawnsRatio[(int)parameters.GamePhase];
        }
    }
}
