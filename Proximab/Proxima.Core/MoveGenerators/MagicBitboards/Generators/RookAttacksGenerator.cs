using Proxima.Core.Boards;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.PatternGenerators;
using System.Collections.Generic;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Generators
{
    public class RookAttacksGenerator
    {
        PermutationsGenerator _permutationsGenerator;
        MagicKeyGenerator _magicKeyGenerator;

        public RookAttacksGenerator()
        {
            _permutationsGenerator = new PermutationsGenerator();
            _magicKeyGenerator = new MagicKeyGenerator();
        }

        public List<FieldPattern> Generate(int fieldIndex)
        {
            var mask = PatternsContainer.RookPattern[fieldIndex];
            var occupancyPermutations = _permutationsGenerator.GetMaskPermutations(mask);
            var fieldPatterns = GetFieldPatterns(fieldIndex, mask, occupancyPermutations);
     
            return fieldPatterns;
        }

        List<FieldPattern> GetFieldPatterns(int fieldIndex, ulong mask, List<ulong> occupancyPermutations)
        {
            var patterns = new List<FieldPattern>();

            foreach(var permutation in occupancyPermutations)
            {
                var rightAttacks = CalculateAttacks(fieldIndex, permutation, new Position(-1, 0));
                var leftAttacks = CalculateAttacks(fieldIndex, permutation, new Position(1, 0));
                var topAttacks = CalculateAttacks(fieldIndex, permutation, new Position(0, 1));
                var bottomAttacks = CalculateAttacks(fieldIndex, permutation, new Position(0, -1));

                var attacks = rightAttacks | leftAttacks | topAttacks | bottomAttacks;

                patterns.Add(new FieldPattern(permutation, attacks));
            }

            return patterns;
        }

        ulong CalculateAttacks(int initialFieldIndex, ulong occupancy, Position shift)
        {
            var attacks = 0ul;
            var currentPosition = BitPositionConverter.ToPosition(initialFieldIndex);

            currentPosition += shift;
            while (currentPosition.IsValid())
            {
                var positionBitIndex = BitPositionConverter.ToBitIndex(currentPosition);
                var bit = 1ul << positionBitIndex;
                attacks |= bit;

                if ((bit & occupancy) != 0)
                {
                    break;
                }

                currentPosition += shift;
            }

            return attacks;
        }
    }
}
