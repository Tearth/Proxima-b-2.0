using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.Evaluation.KingSafety
{
    /// <summary>
    /// Represents a set of methods to evaluate king safety.
    /// </summary>
    /// <remarks>
    /// King safety evaluation result is calculated based on counting all pieces which can
    /// attack any neighbour fields of the king.
    /// </remarks>
    public class KingSafetyCalculator
    {
        /// <summary>
        /// Calculates a king safety evaluation result.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The king safety evaluation result.</returns>
        public int Calculate(Bitboard bitboard)
        {
            var whiteKingSafety = GetAttackedNeighboursValue(bitboard, Color.White);
            var blackKingSafety = GetAttackedNeighboursValue(bitboard, Color.Black);

            return whiteKingSafety - blackKingSafety;
        }

        /// <summary>
        /// Calculates a detailed king safety evaluation result.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The detailed king safety evaluation result.</returns>
        public KingSafetyData CalculateDetailed(Bitboard bitboard)
        {
            return new KingSafetyData
            {
                WhiteAttackedNeighbours = GetAttackedNeighboursValue(bitboard, Color.White),
                BlackAttackedNeighbours = GetAttackedNeighboursValue(bitboard, Color.Black)
            };
        }

        /// <summary>
        /// Calculates a value of pieces that are attacking the neighbour fields of specified king.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The king color.</param>
        /// <returns>The value of the pieces that are attacking king beighbour fields multiplied by the specified ratio.</returns>
        private int GetAttackedNeighboursValue(Bitboard bitboard, Color color)
        {
            var attackedNeightbours = 0;

            var king = bitboard.Pieces[FastArray.GetPieceIndex(color, PieceType.King)];
            var kingIndex = BitOperations.GetBitIndex(king);
            var kingMoves = PatternsContainer.KingPattern[kingIndex];

            while (kingMoves != 0)
            {
                var fieldLSB = BitOperations.GetLSB(kingMoves);
                kingMoves = BitOperations.PopLSB(kingMoves);

                var fieldIndex = BitOperations.GetBitIndex(fieldLSB);
                var attacks = bitboard.Attacks[fieldIndex] & ~bitboard.Occupancy[(int)color];

                attackedNeightbours += BitOperations.Count(attacks);
            }

            return attackedNeightbours * KingSafetyValues.AttackedNeighboursRatio[(int)bitboard.GamePhase];
        }
    }
}
