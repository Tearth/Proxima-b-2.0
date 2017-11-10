using Proxima.Core.Boards;
using Proxima.Core.MoveGenerators.MagicBitboards.Exceptions;
using Proxima.Core.MoveGenerators.PatternGenerators;
using System.Collections.Generic;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks
{
    public class AttacksParser
    {
        RookAttacksGenerator _rookAttacksGenerator;
        BishopAttacksGenerator _bishopAttacksGenerator;

        public AttacksParser()
        {
            _rookAttacksGenerator = new RookAttacksGenerator();
            _bishopAttacksGenerator = new BishopAttacksGenerator();
        }

        public ulong[] ParseRookAttacks()
        {
            var rookAttacks = new ulong[MagicConstants.RookArraySize];

            for (int i = 0; i < 64; i++)
            {
                var patterns = _rookAttacksGenerator.Generate(i);
                var mask = PatternsContainer.RookPattern[i];

                ParsePatterns(patterns, rookAttacks, MagicContainer.RookKeys, mask, i, MagicConstants.RookMovesPerField);
            }

            return rookAttacks;
        }

        public ulong[] ParseBishopAttacks()
        {
            var bishopAttacks = new ulong[MagicConstants.BishopArraySize];

            for (int i = 0; i < 64; i++)
            {
                var patterns = _bishopAttacksGenerator.Generate(i);
                var mask = PatternsContainer.BishopPattern[i];

                ParsePatterns(patterns, bishopAttacks, MagicContainer.BishopKeys, mask, i, MagicConstants.BishopMovesPerField);
            }

            return bishopAttacks;
        }

        void ParsePatterns(List<FieldPattern> patterns, ulong[] attacks, ulong[] keys, ulong mask, int fieldIndex, int patternsPerField)
        {
            var key = keys[fieldIndex];
            foreach(var pattern in patterns)
            {
                var bitsCount = BitOperations.Count(mask);
                var hash = (pattern.Occupancy * key) >> (64 - bitsCount);
                var index = (patternsPerField * fieldIndex) + (int)hash;
                
                if (attacks[index] != 0 && attacks[index] != pattern.Attacks)
                {
                    throw new InvalidMagicKeyException();
                }

                attacks[index] = pattern.Attacks;
            }
        }
    }
}
