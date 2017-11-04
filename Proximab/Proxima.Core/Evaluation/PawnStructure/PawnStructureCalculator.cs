using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Evaluation.PawnStructure
{
    public class PawnStructureCalculator
    {
        public PawnStructureResult Calculate(EvaluationParameters parameters)
        {
            return new PawnStructureResult()
            {
                WhiteDoublePawns = GetDoublePawns(Color.White, parameters),
                BlackDoublePawns = GetDoublePawns(Color.Black, parameters)
            };
        }

        int GetDoublePawns(Color color, EvaluationParameters parameters)
        {
            var doublePawns = 0;

            var pawns = parameters.Pieces[FastArray.GetPieceIndex(color, PieceType.Pawn)];
            var file = BitConstants.HFile;

            for(int i=0; i<7; i++)
            {
                file <<= 1;

                var pawnsInFile = pawns & file;
                var pawnLSB = BitOperations.GetLSB(ref pawnsInFile);

                if(pawnsInFile != 0)
                {
                    doublePawns += BitOperations.Count(pawnsInFile);
                }
            }

            return doublePawns * PawnStructureValues.DoublePawnsPenalty[(int)parameters.GamePhase];
        }
    }
}
