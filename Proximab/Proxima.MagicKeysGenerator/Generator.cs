using Proxima.Core.Boards;
using Proxima.Core.MoveGenerators.MagicBitboards.Attacks.Generators;
using Proxima.Core.MoveGenerators.MagicBitboards.Keys;
using Proxima.Core.MoveGenerators.PatternGenerators;
using System;

namespace Proxima.MagicKeysGenerator
{
    /// <summary>
    /// Generator of magic keys for bishop and rook. Because generating keys for
    /// every field can take a few seconds, it cannot be done every time when chess engine is
    /// starting. Therefore all magic keys are once saved to the specified files and loaded
    /// every time when main app is starting which is of course a lot faster.
    /// </summary>
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

        /// <summary>
        /// Returns 64-element array of magic keys for rook
        /// </summary>
        public ulong[] GetRookKeys()
        {
            return GetKeys(new RookAttacksGenerator(), PatternsContainer.RookPattern);
        }

        /// <summary>
        /// Returns 64-element array of magic keys for bishop
        /// </summary>
        public ulong[] GetBishopKeys()
        {
            return GetKeys(new BishopAttacksGenerator(), PatternsContainer.BishopPattern);
        }

        /// <summary>
        /// Returns 64-element array of magic keys for the specified piece:
        ///  - Rook:    RookAttacksGenerator and PatternsContainer.RookPattern
        ///  - Bishop:  BishopAttacksGenerator and PatternsContainer.BishopPattern
        /// </summary>
        ulong[] GetKeys(IAttacksGenerator attacksGenerator, ulong[] pieceAttackPatterns)
        {
            var keys = new ulong[64];

            for(int fieldIndex=0; fieldIndex < 64; fieldIndex++)
            {
                var mask = pieceAttackPatterns[fieldIndex];
                var maskLength = BitOperations.Count(mask);

                var patterns = attacksGenerator.Generate(fieldIndex);

                keys[fieldIndex] = _magicKeyGenerator.GenerateKey(patterns, maskLength);
                DisplayStatus(fieldIndex, keys[fieldIndex], maskLength, patterns.Count);
            }

            return keys;
        }

        /// <summary>
        /// Displays information about magic key in console in more friendly-user way.
        /// </summary>
        void DisplayStatus(int fieldIndex, ulong magicKey, int maskLength, int patternLength)
        {
            Console.WriteLine($"Key {fieldIndex} = {magicKey}, " +
                              $"mask size: {maskLength}, " +
                              $"patterns size: {patternLength}");
        }
    }
}
