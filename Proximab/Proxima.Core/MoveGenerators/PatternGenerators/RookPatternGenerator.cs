using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.MoveGenerators.PatternGenerators
{
    /// <summary>
    /// Represents a set of methods to generate rook patterns.
    /// </summary>
    public class RookPatternGenerator
    {
        /// <summary>
        /// Generates rook patterns for every field.
        /// </summary>
        /// <returns>The 64-element array with patterns.</returns>
        public ulong[] Generate()
        {
            var predefinedMoves = new ulong[64];

            for (var i = 0; i < 64; i++)
            {
                var fieldBit = 1ul << i;
                var position = BitPositionConverter.ToPosition(i);

                var filePattern = GetFilePattern(position) & ~BitConstants.TopBottomEdge;
                var rankPattern = GetRankPattern(position) & ~BitConstants.RightLeftEdge;

                predefinedMoves[i] = ~fieldBit & (filePattern | rankPattern);
            }

            return predefinedMoves;
        }

        /// <summary>
        /// Calculates file pattern for the specified position.
        /// </summary>
        /// <param name="position">The field position.</param>
        /// <returns>The file pattern for the specified position.</returns>
        private ulong GetFilePattern(Position position)
        {
            return BitConstants.AFile >> (position.X - 1);
        }

        /// <summary>
        /// Calculates rank pattern for the specified position.
        /// </summary>
        /// <param name="position">The field position.</param>
        /// <returns>The rank pattern for the specified position.</returns>
        private ulong GetRankPattern(Position position)
        {
            return BitConstants.ARank << ((position.Y - 1) << 3);
        }
    }
}
