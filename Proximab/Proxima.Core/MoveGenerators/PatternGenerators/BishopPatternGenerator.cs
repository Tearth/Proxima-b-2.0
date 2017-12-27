using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.MoveGenerators.PatternGenerators
{
    /// <summary>
    /// Represents a set of methods to generate bishop patterns.
    /// </summary>
    public class BishopPatternGenerator
    {
        /// <summary>
        /// Generates bishop patterns for every field.
        /// </summary>
        /// <returns>The 64-element array with patterns.</returns>
        public ulong[] Generate()
        {
            var predefinedMoves = new ulong[64];

            for (var i = 0; i < 64; i++)
            {
                var rightTopPattern = CalculatePattern(i, new Position(-1, 1));
                var leftTopPattern = CalculatePattern(i, new Position(1, 1));
                var rightBottomPattern = CalculatePattern(i, new Position(-1, -1));
                var leftBottomPattern = CalculatePattern(i, new Position(1, -1));

                var pattern = rightTopPattern | leftTopPattern | rightBottomPattern | leftBottomPattern;
                pattern &= BitConstants.BitboardWithoutEdges;

                predefinedMoves[i] = pattern;
            }

            return predefinedMoves;
        }

        /// <summary>
        /// Calculates pattern for the specified field and shift.
        /// </summary>
        /// <param name="fieldIndex">The index field.</param>
        /// <param name="shift">The shift (the direction in which the calculating is made).</param>
        /// <returns>The pattern for the specified field.</returns>
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
