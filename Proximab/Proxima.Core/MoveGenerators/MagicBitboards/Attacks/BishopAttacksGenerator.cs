using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.PatternGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks
{
    public class BishopAttacksGenerator
    {
        PermutationsGenerator _permutationsGenerator;
        AttacksGenerator _attacksGenerator;

        public BishopAttacksGenerator()
        {
            _permutationsGenerator = new PermutationsGenerator();
            _attacksGenerator = new AttacksGenerator();
        }

        public List<FieldPattern> Generate(int fieldIndex)
        {
            var mask = PatternsContainer.BishopPattern[fieldIndex];
            var occupancyPermutations = _permutationsGenerator.GetMaskPermutations(mask);
            var fieldPatterns = GetFieldPatterns(fieldIndex, mask, occupancyPermutations);

            return fieldPatterns;
        }

        List<FieldPattern> GetFieldPatterns(int fieldIndex, ulong mask, List<ulong> occupancyPermutations)
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
