using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Evaluation.PawnStructure.Doubled
{
    public class DoubledPawnsCalculator
    {
        public int GetDoubledPawns(Color color, EvaluationParameters parameters)
        {
            var doubledPawns = 0;
            var pawns = parameters.Pieces[FastArray.GetPieceIndex(color, PieceType.Pawn)];

            for (int i = 0; i < 8; i++)
            {
                var file = BitConstants.HFile << i;

                var pawnsInFile = pawns & file;
                var pawnLSB = BitOperations.GetLSB(ref pawnsInFile);

                if (pawnsInFile != 0)
                {
                    doubledPawns += BitOperations.Count(pawnsInFile);
                }
            }

            return doubledPawns * PawnStructureValues.DoubledPawnsPenalty[(int)parameters.GamePhase];
        }
    }
}
