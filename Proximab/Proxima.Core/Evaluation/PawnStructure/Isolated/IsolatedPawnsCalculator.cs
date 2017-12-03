using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Evaluation.PawnStructure.Isolated
{
    /// <summary>
    /// Represents a set of methods to evaluate doubled pawns.
    /// </summary>
    /// <remarks>
    /// We can talk about pawn as isoleted when there is no any same-color pawns on neighbour files. It's extremally
    /// harmful, because these pawns are very hard to defend and can be easily killed by the enemy pieces.
    /// </remarks>
    public class IsolatedPawnsCalculator
    {
        /// <summary>
        /// Calculates a isolated pawns evaluation result for the specified player by adding number of pawns
        /// without any same-color pawns at neighbour files.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The player color.</param>
        /// <returns>The isolated pawns evaluation result for the specified player.</returns>
        public int GetIsolatedPawnsValue(Bitboard bitboard, Color color)
        {
            var isolatedPawns = 0;
            var pawns = bitboard.Pieces[FastArray.GetPieceIndex(color, PieceType.Pawn)];

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

            return isolatedPawns * PawnStructureValues.IsolatededPawnsRatio[(int)bitboard.GamePhase];
        }
    }
}
