using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Evaluation.PawnStructure.Doubled
{
    /// <summary>
    /// Represents a set of methods to evaluate doubled pawns.
    /// </summary>
    /// <remarks>
    /// By the "doubled pawns" term we can assume two or more pawns on the same file. These pawns are really hard
    /// to defend and their mobility is seriously limited.
    /// </remarks>
    public class DoubledPawnsCalculator
    {
        /// <summary>
        /// Calculates a doubled pawns evaluation result for the specified player by adding number of pawns
        /// on the same files.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The player color.</param>
        /// <returns>The doubled pawns evaluation result for the specified player.</returns>
        public int GetDoubledPawnsValue(Bitboard bitboard, Color color)
        {
            var doubledPawns = 0;
            var pawns = bitboard.Pieces[FastArray.GetPieceIndex(color, PieceType.Pawn)];

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

            return doubledPawns * PawnStructureValues.DoubledPawnsRatio[(int)bitboard.GamePhase];
        }
    }
}
