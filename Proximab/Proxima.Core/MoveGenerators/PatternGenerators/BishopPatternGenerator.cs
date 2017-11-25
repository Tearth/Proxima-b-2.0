using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.MoveGenerators.PatternGenerators
{
    public class BishopPatternGenerator
    {
        public ulong[] Generate()
        {
            var predefinedMoves = new ulong[64];

            for (int i = 0; i < 64; i++)
            {
                var rightTopPattern = CalculatePattern(i, new Position(-1, 1));
                var leftTopPattern = CalculatePattern(i, new Position(1, 1));
                var rightBottomPattern = CalculatePattern(i, new Position(-1, -1));
                var leftBottomPattern = CalculatePattern(i, new Position(1, -1));

                var pattern = rightTopPattern | leftTopPattern | rightBottomPattern | leftBottomPattern;
                pattern &= BitConstants.BitBoardWithoutEdges;

                predefinedMoves[i] = pattern;
            }

            return predefinedMoves;
        }

        public ulong CalculatePattern(int fieldIndex, Position shift)
        {
            var attacks = 0ul;
            var currentPosition = BitPositionConverter.ToPosition(fieldIndex);

            currentPosition += shift;
            while (currentPosition.IsValid())
            {
                var positionBitIndex = BitPositionConverter.ToBitIndex(currentPosition);
                var bit = 1ul << positionBitIndex;
                attacks |= bit;

                currentPosition += shift;
            }

            return attacks;
        }
    }
}
