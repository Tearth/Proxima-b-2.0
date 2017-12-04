using System.Collections.Generic;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks.Generators
{
    /// <summary>
    /// Represents a set of methods to generate bishop attacks.
    /// </summary>
    public class BishopAttacksGenerator : IAttacksGenerator
    {
        private PermutationsGenerator _permutationsGenerator;
        private AvailableMovesGenerator _availableMovesGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BishopAttacksGenerator"/> class.
        /// </summary>
        public BishopAttacksGenerator()
        {
            _permutationsGenerator = new PermutationsGenerator();
            _availableMovesGenerator = new AvailableMovesGenerator();
        }

        /// <summary>
        /// Generates a list of attack patterns for the specified field.
        /// </summary>
        /// <param name="fieldIndex">The field index.</param>
        /// <returns>The list of the patterns for the specified field.</returns>
        public List<FieldAttackPattern> Generate(int fieldIndex)
        {
            var patterns = new List<FieldAttackPattern>();
            var mask = PatternsContainer.BishopPattern[fieldIndex];
            var occupancyPermutations = _permutationsGenerator.GenerateMaskPermutations(mask);

            foreach (var permutation in occupancyPermutations)
            {
                var topRightAttacks = _availableMovesGenerator.Calculate(fieldIndex, permutation, new Position(-1, 1));
                var topLeftAttacks = _availableMovesGenerator.Calculate(fieldIndex, permutation, new Position(1, 1));
                var bottomRightAttacks = _availableMovesGenerator.Calculate(fieldIndex, permutation, new Position(-1, -1));
                var bottomLeftAttacks = _availableMovesGenerator.Calculate(fieldIndex, permutation, new Position(1, -1));

                var attacks = topRightAttacks | topLeftAttacks | bottomRightAttacks | bottomLeftAttacks;

                patterns.Add(new FieldAttackPattern(permutation, attacks));
            }

            return patterns;
        }
    }
}
