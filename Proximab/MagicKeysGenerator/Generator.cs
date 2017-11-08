using Proxima.Core;
using Proxima.Core.Boards;
using Proxima.Core.MoveGenerators.MagicBitboards.Attacks;
using Proxima.Core.MoveGenerators.MagicBitboards.Keys;
using Proxima.Core.MoveGenerators.PatternGenerators;
using System;

namespace MagicKeysGenerator
{
    internal class Generator
    {
        MagicKeyGenerator _magicKeyGenerator;
        RookAttacksGenerator _rookAttacksGenerator;

        public Generator()
        {
            _magicKeyGenerator = new MagicKeyGenerator();
            _rookAttacksGenerator = new RookAttacksGenerator();

            PatternsContainer.GeneratePatterns();
        }

        public ulong[] GetKeys()
        {
            var keys = new ulong[64];

            for(int i=0; i<64; i++)
            {
                var mask = PatternsContainer.RookPattern[i];
                var maskSize = BitOperations.Count(mask);
                var patterns = _rookAttacksGenerator.Generate(i);

                keys[i] = _magicKeyGenerator.GenerateKey(patterns, mask);

                Console.WriteLine($"Key {i} = {keys[i]}, " +
                                  $"mask size: {maskSize}, " +
                                  $"patterns size: {patterns.Count}");
            }

            return keys;
        }
    }
}
