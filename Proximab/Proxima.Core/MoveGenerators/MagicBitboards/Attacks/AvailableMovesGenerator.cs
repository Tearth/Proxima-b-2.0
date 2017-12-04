using Proxima.Core.Boards;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks
{
    /// <summary>
    /// Represents a set of methods to generate available moves for the specified bitboards.
    /// </summary>
    public class AvailableMovesGenerator
    {
        /// <summary>
        /// Generates available moves for the specified bitboard and shift.
        /// </summary>
        /// <param name="initialFieldIndex">The initial field index.</param>
        /// <param name="occupancy">The bitboard occupancy.</param>
        /// <param name="shift">The shift (direction of available moves generating).</param>
        /// <returns>The available moves bitboard (where 1 means possible position, otherwise 0).</returns>
        public ulong Calculate(int initialFieldIndex, ulong occupancy, Position shift)
        {
            var attacks = 0ul;
            var bit = 0ul;
            var currentPosition = BitPositionConverter.ToPosition(initialFieldIndex);

            currentPosition += shift;
            while (currentPosition.IsValid() && (bit & occupancy) == 0)
            {
                var positionBitIndex = BitPositionConverter.ToBitIndex(currentPosition);

                bit = 1ul << positionBitIndex;
                attacks |= bit;

                currentPosition += shift;
            }

            return attacks;
        }
    }
}
