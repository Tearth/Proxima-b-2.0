using Proxima.Core.Boards;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks
{
    public class AvailableMovesGenerator
    {
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
