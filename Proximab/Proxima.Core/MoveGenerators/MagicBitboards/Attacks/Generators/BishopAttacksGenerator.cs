using System.Collections.Generic;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks.Generators
{
    public class BishopAttacksGenerator : IAttacksGenerator
    {
        private PermutationsGenerator _permutationsGenerator;
        private AvailableMovesGenerator _attacksGenerator;

        public BishopAttacksGenerator()
        {
            _permutationsGenerator = new PermutationsGenerator();
            _attacksGenerator = new AvailableMovesGenerator();
        }

        public List<FieldPattern> Generate(int fieldIndex)
        {
            var mask = PatternsContainer.BishopPattern[fieldIndex];
            var occupancyPermutations = _permutationsGenerator.GetMaskPermutations(mask);
            var fieldPatterns = GetFieldPatterns(fieldIndex, mask, occupancyPermutations);

            return fieldPatterns;
        }

        private List<FieldPattern> GetFieldPatterns(int fieldIndex, ulong mask, List<ulong> occupancyPermutations)
        {
            var patterns = new List<FieldPattern>();

            foreach (var permutation in occupancyPermutations)
            {
                var topRightAttacks = _attacksGenerator.Calculate(fieldIndex, permutation, new Position(-1, 1));
                var topLeftAttacks = _attacksGenerator.Calculate(fieldIndex, permutation, new Position(1, 1));
                var bottomRightAttacks = _attacksGenerator.Calculate(fieldIndex, permutation, new Position(-1, -1));
                var bottomLeftAttacks = _attacksGenerator.Calculate(fieldIndex, permutation, new Position(1, -1));

                var attacks = topRightAttacks | topLeftAttacks | bottomRightAttacks | bottomLeftAttacks;

                patterns.Add(new FieldPattern(permutation, attacks));
            }

            return patterns;
        }
    }
}
