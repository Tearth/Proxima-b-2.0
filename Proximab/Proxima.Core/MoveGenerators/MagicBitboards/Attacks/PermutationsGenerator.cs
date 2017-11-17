using Proxima.Core.Boards;
using System.Collections.Generic;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks
{
    public class PermutationsGenerator
    {
        public List<ulong> GetMaskPermutations(ulong mask)
        {
            var permuatations = new List<ulong>();
            var bitIndexes = GetBitIndexes(mask);
            var permutationsCount = 1 << bitIndexes.Count;

            for (int i = 0; i<permutationsCount; i++)
            {
                permuatations.Add(CalculatePermutation(i, bitIndexes));
            }

            return permuatations;
        }

        List<int> GetBitIndexes(ulong mask)
        {
            var bitIndexes = new List<int>();

            while(mask != 0)
            {
                var lsb = BitOperations.GetLSB(mask);
                mask = BitOperations.PopLSB(mask);

                var index = BitOperations.GetBitIndex(lsb);

                bitIndexes.Add(index);
            }

            return bitIndexes;
        }

        ulong CalculatePermutation(int permutationIndex, List<int> bitIndexes)
        {
            var permutation = 0ul;

            while(permutationIndex != 0)
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
