using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Evaluation.PawnStructure.Doubled
{
    public class DoubledPawnsCalculator
    {
        public int GetDoubledPawnsValue(Color color, GamePhase gamePhase, BitBoard bitBoard)
        {
            var doubledPawns = 0;
            var pawns = bitBoard.Pieces[FastArray.GetPieceIndex(color, PieceType.Pawn)];

            for (int i = 0; i < 8; i++)
            {
                var file = BitConstants.HFile << i;

                var pawnsInFile = pawns & file;
                var pawnLSB = BitOperations.GetLSB(pawnsInFile);
                pawnsInFile = BitOperations.PopLSB(pawnsInFile);

                if (pawnsInFile != 0)
                {
                    doubledPawns += BitOperations.Count(pawnsInFile);
                }
            }

            return doubledPawns * PawnStructureValues.DoubledPawnsRatio[(int)gamePhase];
        }
    }
}
