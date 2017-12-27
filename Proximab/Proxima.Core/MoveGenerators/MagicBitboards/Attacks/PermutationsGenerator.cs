using System.Collections.Generic;
using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks
{
    /// <summary>
    /// Represents a set of methods to generate bitboard permutations.
    /// </summary>
    public class PermutationsGenerator
    {
        /// <summary>
        /// Generates a list of bitboard permutations (but only these bits where mask was set).
        /// </summary>
        /// <param name="mask">The mask (bits where permutation will be applied)</param>
        /// <returns>The list of the permutations for the specified mask.</returns>
        public List<ulong> GenerateMaskPermutations(ulong mask)
        {
            var permuatations = new List<ulong>();
            var bitIndexes = GenerateBitIndexes(mask);
            var permutationsCount = 1 << bitIndexes.Count;

            for (int i = 0; i < permutationsCount; i++)
            {
                permuatations.Add(GeneratePermutation(i, bitIndexes));
            }

            return permuatations;
        }

        /// <summary>
        /// Generates a list of bit indexes (all bits for those mask was set).
        /// </summary>
        /// <param name="mask">The mask (bits where permutation will be applied)</param>
        /// <returns>The list of bit indexes for the specifie mask.</returns>
        private List<int> GenerateBitIndexes(ulong mask)
        {
            var bitIndexes = new List<int>();

            while (mask != 0)
            {
                var lsb = BitOperations.GetLSB(mask);
                mask = BitOperations.PopLSB(mask);

                var index = BitOperations.GetBitIndex(lsb);

                bitIndexes.Add(index);
            }

            return bitIndexes;
        }

        /// <summary>
        /// Generates permutation.
        /// </summary>
        /// <param name="permutationIndex">The permutation index (can't be bigger than 2^{number of mask bits}).</param>
        /// <param name="bitIndexes">The list of bit indexes to apply permutation.</param>
        /// <returns>The permutation bitboard.</returns>
        private ulong GeneratePermutation(int permutationIndex, List<int> bitIndexes)
        {
            var permutation = 0ul;

            while (permutationIndex != 0)
            {
                var lsb = BitOperations.GetLSB(permutationIndex);
                permutationIndex = BitOperations.PopLSB(permutationIndex);

                var lsbIndex = BitOperations.GetBitIndex((ulong)lsb);

                permutation |= 1ul << bitIndexes[lsbIndex];
            }

            return permutation;
        }
    }
}
