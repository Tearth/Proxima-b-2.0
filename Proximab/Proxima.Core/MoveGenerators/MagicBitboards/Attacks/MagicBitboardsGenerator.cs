using System.Collections.Generic;
using Proxima.Core.MoveGenerators.MagicBitboards.Attacks.Generators;
using Proxima.Core.MoveGenerators.MagicBitboards.Exceptions;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks
{
    /// <summary>
    /// Represents a set of methods to generate magic bitboards. 
    /// </summary>
    public class MagicBitboardsGenerator
    {
        /// <summary>
        /// Generates magic bitboards for the specified parameters.
        /// </summary>
        /// <param name="attacksGenerator">The attacks generator.</param>
        /// <param name="patternBitsCount">The array of pattern set bits count.</param>
        /// <param name="keys">The array of magic keys.</param>
        /// <returns>The array of fields containing a list of possible attacks for bishop (accessed by magic keys).</returns>
        public ulong[][] GenerateMagicBitboards(IAttacksGenerator attacksGenerator, int[] patternBitsCount, ulong[] keys)
        {
            var bishopAttacks = new ulong[64][];

            for (int i = 0; i < 64; i++)
            {
                var attacks = attacksGenerator.Generate(i);
                var maskBitsCount = patternBitsCount[i];

                bishopAttacks[i] = GenerateMagicAttacks(attacks, keys, maskBitsCount, i);
            }

            return bishopAttacks;
        }

        /// <summary>
        /// Generates a magic attacks for the specified field.
        /// </summary>
        /// <param name="patterns">The list of available patterns for the specified field.</param>
        /// <param name="keys">The array of the magic keys.</param>
        /// <param name="patternBitsCount">The pattern set bits count.</param>
        /// <param name="fieldIndex">The field index.</param>
        /// <returns>The array of possible attacks for the specified field.</returns>
        private ulong[] GenerateMagicAttacks(List<FieldAttackPattern> patterns, ulong[] keys, int patternBitsCount, int fieldIndex)
        {
            var attacks = new ulong[1 << patternBitsCount];
            var key = keys[fieldIndex];

            foreach (var pattern in patterns)
            {
                var hash = (pattern.Occupancy * key) >> (64 - patternBitsCount);
                if (attacks[hash] != 0 && attacks[hash] != pattern.Attacks)
                {
                    throw new InvalidMagicKeyException();
                }

                attacks[hash] = pattern.Attacks;
            }

            return attacks;
        }
    }
}
