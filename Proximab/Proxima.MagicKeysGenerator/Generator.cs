using Proxima.Core;
using Proxima.Core.Boards;
using Proxima.Core.MoveGenerators.MagicBitboards.Attacks;
using Proxima.Core.MoveGenerators.MagicBitboards.Keys;
using Proxima.Core.MoveGenerators.PatternGenerators;
using System;
using System.Collections.Generic;

namespace Proxima.MagicKeysGenerator
{
    internal class Generator
    {
        MagicKeyGenerator _magicKeyGenerator;
        RookAttacksGenerator _rookAttacksGenerator;
        BishopAttacksGenerator _bishopAttacksGenerator;

        public Generator()
        {
            _magicKeyGenerator = new MagicKeyGenerator();
            _rookAttacksGenerator = new RookAttacksGenerator();
            _bishopAttacksGenerator = new BishopAttacksGenerator();

            PatternsContainer.GeneratePatterns();
        }

        public ulong[] GetRookKeys()
        {
            var keys = new ulong[64];

            for(int i=0; i<64; i++)
            {
                var patterns = GetRookPatterns(i, out var mask, out var maskSize);

                keys[i] = _magicKeyGenerator.GenerateKey(patterns, mask);
                DisplayStatus(i, keys[i], maskSize, patterns.Count);
            }

            return keys;
        }

        public ulong[] GetBishopKeys()
        {
            var keys = new ulong[64];

            for (int i = 0; i < 64; i++)
            {
                var patterns = GetBishopPatterns(i, out var mask, out var maskSize);

                keys[i] = _magicKeyGenerator.GenerateKey(patterns, mask);
                DisplayStatus(i, keys[i], maskSize, patterns.Count);
            }

            return keys;
        }

        List<FieldPattern> GetRookPatterns(int fieldIndex, out ulong mask, out int maskSize)
        {
            mask = PatternsContainer.RookPattern[fieldIndex];
            maskSize = BitOperations.Count(mask);

            return _rookAttacksGenerator.Generate(fieldIndex);
        }

        List<FieldPattern> GetBishopPatterns(int fieldIndex, out ulong mask, out int maskSize)
        {
            mask = PatternsContainer.BishopPattern[fieldIndex];
            maskSize = BitOperations.Count(mask);

            return _bishopAttacksGenerator.Generate(fieldIndex);
        }

        void DisplayStatus(int fieldIndex, ulong magicKey, int maskSize, int patternSize)
        {
            Console.WriteLine($"Key {fieldIndex} = {magicKey}, " +
                              $"mask size: {maskSize}, " +
                              $"patterns size: {patternSize}");
        }
    }
}
