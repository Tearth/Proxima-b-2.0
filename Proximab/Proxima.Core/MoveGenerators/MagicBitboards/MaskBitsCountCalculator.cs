using Proxima.Core.Commons.BitHelpers;

namespace Proxima.Core.MoveGenerators.MagicBitboards
{
    /// <summary>
    /// Represents a set of methods to calculating mask bits count.
    /// </summary>
    public class MaskBitsCountCalculator
    {
        /// <summary>
        /// Calculates set bits count for the specified patterns.
        /// </summary>
        /// <param name="patterns">The array of patterns.</param>
        /// <returns>The array of set bits count.</returns>
        public int[] Calculate(ulong[] patterns)
        {
            var bitsCountArray = new int[64];

            for (int i = 0; i < 64; i++)
            {
                bitsCountArray[i] = BitOperations.Count(patterns[i]);
            }

            return bitsCountArray;
        }
    }
}
