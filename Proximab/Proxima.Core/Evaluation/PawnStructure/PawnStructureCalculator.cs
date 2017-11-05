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
                WhiteDoubledPawns = GetDoubledPawns(Color.White, parameters),
                BlackDoubledPawns = GetDoubledPawns(Color.Black, parameters),

                WhiteIsolatedPawns = GetIsolatedPawns(Color.White, parameters),
                BlackIsolatedPawns = GetIsolatedPawns(Color.Black, parameters)
            };
        }

        int GetDoubledPawns(Color color, EvaluationParameters parameters)
        {
            var doubledPawns = 0;
            var pawns = parameters.Pieces[FastArray.GetPieceIndex(color, PieceType.Pawn)];
            
            for(int i=0; i<8; i++)
            {
                var file = BitConstants.HFile << i;

                var pawnsInFile = pawns & file;
                var pawnLSB = BitOperations.GetLSB(ref pawnsInFile);

                if(pawnsInFile != 0)
                {
                    doubledPawns += BitOperations.Count(pawnsInFile);
                }
            }

            return doubledPawns * PawnStructureValues.DoubledPawnsPenalty[(int)parameters.GamePhase];
        }

        int GetIsolatedPawns(Color color, EvaluationParameters parameters)
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

            return isolatedPawns * PawnStructureValues.IsolatededPawnsPenalty[(int)parameters.GamePhase];
        }
    }
}
