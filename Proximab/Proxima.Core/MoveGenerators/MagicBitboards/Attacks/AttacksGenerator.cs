using Proxima.Core.Boards;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks
{
    public class AttacksGenerator
    {
        public ulong Calculate(int initialFieldIndex, ulong occupancy, Position shift)
        {
            var attacks = 0ul;
            var currentPosition = BitPositionConverter.ToPosition(initialFieldIndex);

            currentPosition += shift;
            while (currentPosition.IsValid())
            {
                var positionBitIndex = BitPositionConverter.ToBitIndex(currentPosition);

                var bit = 1ul << positionBitIndex;
                attacks |= bit;

                if ((bit & occupancy) != 0)
                {
                    break;
                }

                currentPosition += shift;
            }

            return attacks;
        }
    }
}
