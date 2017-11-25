using System.Collections.Generic;
using Proxima.Core.MoveGenerators.MagicBitboards.Attacks.Generators;
using Proxima.Core.MoveGenerators.MagicBitboards.Exceptions;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks
{
    public class AttacksParser
    {
        private RookAttacksGenerator _rookAttacksGenerator;
        private BishopAttacksGenerator _bishopAttacksGenerator;

        public AttacksParser()
        {
            _rookAttacksGenerator = new RookAttacksGenerator();
            _bishopAttacksGenerator = new BishopAttacksGenerator();
        }

        public ulong[][] ParseRookAttacks()
        {
            var rookAttacks = new ulong[64][];

            for (int i = 0; i < 64; i++)
            {
                var patterns = _rookAttacksGenerator.Generate(i);
                var mask = PatternsContainer.RookPattern[i];
                var maskBitsCount = MagicContainer.RookMaskBitsCount[i];

                rookAttacks[i] = ParsePatterns(patterns, MagicContainer.RookKeys, maskBitsCount, i, MagicConstants.RookMovesPerField);
            }

            return rookAttacks;
        }

        public ulong[][] ParseBishopAttacks()
        {
            var bishopAttacks = new ulong[64][];

            for (int i = 0; i < 64; i++)
            {
                var patterns = _bishopAttacksGenerator.Generate(i);
                var mask = PatternsContainer.BishopPattern[i];
                var maskBitsCount = MagicContainer.BishopMaskBitsCount[i];

                bishopAttacks[i] = ParsePatterns(patterns, MagicContainer.BishopKeys, maskBitsCount, i, MagicConstants.BishopMovesPerField);
            }

            return bishopAttacks;
        }

        private ulong[] ParsePatterns(List<FieldPattern> patterns, ulong[] keys, int maskBitsCount, int fieldIndex, int patternsPerField)
        {
            var attacks = new ulong[1 << maskBitsCount];
            var key = keys[fieldIndex];

            foreach (var pattern in patterns)
            {
                var hash = (pattern.Occupancy * key) >> (64 - maskBitsCount);
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
