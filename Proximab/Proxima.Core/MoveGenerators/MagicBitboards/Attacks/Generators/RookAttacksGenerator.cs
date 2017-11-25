using System.Collections.Generic;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks.Generators
{
    public class RookAttacksGenerator : IAttacksGenerator
    {
        private PermutationsGenerator _permutationsGenerator;
        private AvailableMovesGenerator _attacksGenerator;

        public RookAttacksGenerator()
        {
            _permutationsGenerator = new PermutationsGenerator();
            _attacksGenerator = new AvailableMovesGenerator();
        }

        public List<FieldPattern> Generate(int fieldIndex)
        {
            var mask = PatternsContainer.RookPattern[fieldIndex];
            var occupancyPermutations = _permutationsGenerator.GetMaskPermutations(mask);
            var fieldPatterns = GetFieldPatterns(fieldIndex, mask, occupancyPermutations);

            return fieldPatterns;
        }

        private List<FieldPattern> GetFieldPatterns(int fieldIndex, ulong mask, List<ulong> occupancyPermutations)
        {
            var patterns = new List<FieldPattern>();

            foreach (var permutation in occupancyPermutations)
            {
                var rightAttacks = _attacksGenerator.Calculate(fieldIndex, permutation, new Position(-1, 0));
                var leftAttacks = _attacksGenerator.Calculate(fieldIndex, permutation, new Position(1, 0));
                var topAttacks = _attacksGenerator.Calculate(fieldIndex, permutation, new Position(0, 1));
                var bottomAttacks = _attacksGenerator.Calculate(fieldIndex, permutation, new Position(0, -1));

                var attacks = rightAttacks | leftAttacks | topAttacks | bottomAttacks;

                patterns.Add(new FieldPattern(permutation, attacks));
            }

            return patterns;
        }
    }
}
