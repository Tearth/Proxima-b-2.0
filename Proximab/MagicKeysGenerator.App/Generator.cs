using System;
using Proxima.Core.Boards;
using Proxima.Core.MoveGenerators.MagicBitboards.Attacks.Generators;
using Proxima.Core.MoveGenerators.MagicBitboards.Keys;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace MagicKeysGenerator.App
{
    /// <summary>
    /// Represents set of methods for generating magic keys for bishop and rook.
    /// </summary>
    /// <remarks>
    /// Because generating keys for every field can take a few seconds, it cannot be done every
    /// time when chess engine is starting. Therefore all magic keys are once saved to 
    /// the specified files and loaded every time when main app is starting which is of course 
    /// a lot faster.
    /// </remarks>
    internal class Generator
    {
        private MagicKeyGenerator _magicKeyGenerator;
        private RookAttacksGenerator _rookAttacksGenerator;
        private BishopAttacksGenerator _bishopAttacksGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class.
        /// </summary>
        public Generator()
        {
            _magicKeyGenerator = new MagicKeyGenerator();
            _rookAttacksGenerator = new RookAttacksGenerator();
            _bishopAttacksGenerator = new BishopAttacksGenerator();

            PatternsContainer.Init();
        }

        /// <summary>
        /// Calculates magic keys for rook.
        /// </summary>
        /// <returns>
        /// The 64-element array of magic keys.
        /// </returns>
        public ulong[] GetRookKeys()
        {
            return GetKeys(new RookAttacksGenerator(), PatternsContainer.RookPattern);
        }

        /// <summary>
        /// Calculates magic keys for bishop.
        /// </summary>
        /// <returns>
        /// The 64-element array of magic keys.
        /// </returns>
        public ulong[] GetBishopKeys()
        {
            return GetKeys(new BishopAttacksGenerator(), PatternsContainer.BishopPattern);
        }

        /// <summary>
        /// Calculates magic keys for the specified piece.
        /// </summary>
        /// <param name="attacksGenerator">RookAttacksGenerator for rook or BishopAttacksGenerator for bishop</param>
        /// <param name="pieceAttackPatterns">BishopAttacksGenerator for rook PatternsContainer.BishopPattern for bishop</param>
        /// <returns>Array of magic keys for the specified piece</returns>
        private ulong[] GetKeys(IAttacksGenerator attacksGenerator, ulong[] pieceAttackPatterns)
        {
            var keys = new ulong[64];

            for (int fieldIndex = 0; fieldIndex < 64; fieldIndex++)
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
        /// <param name="fieldIndex">Index of the currently processed field</param>
        /// <param name="magicKey">Calculated magic key</param>
        /// <param name="maskLength">Length (number of set bits) of the mask.</param>
        /// <param name="patternLength">Length (number of set bits) of the pattern.</param>
        private void DisplayStatus(int fieldIndex, ulong magicKey, int maskLength, int patternLength)
        {
            Console.WriteLine($"Key {fieldIndex} = {magicKey}, " +
                              $"mask size: {maskLength}, " +
                              $"patterns size: {patternLength}");
        }
    }
}
