﻿using Proxima.Core.Commons.BitHelpers;

namespace Proxima.Core.MoveGenerators.PatternGenerators
{
    /// <summary>
    /// Represents a set of methods to generate king patterns.
    /// </summary>
    public class KingPatternGenerator
    {
        /// <summary>
        /// Generates king patterns for every field.
        /// </summary>
        /// <returns>The 64-element array with patterns.</returns>
        public ulong[] Generate()
        {
            var predefinedMoves = new ulong[64];
            var bitPosition = 1ul;

            for (var i = 0; i < 64; i++)
            {
                predefinedMoves[i] = GetPattern(bitPosition);
                bitPosition <<= 1;
            }

            return predefinedMoves;
        }

        /// <summary>
        /// Generates expanded king pattern (5x5 square around king).
        /// </summary>
        /// <returns>The 64-element array with patterns.</returns>
        public ulong[] GenerateExpanded()
        {
            var normalPattern = Generate();
            var expandedPattern = new ulong[64];

            for (var i = 0; i < 64; i++)
            {
                while (normalPattern[i] != 0)
                {
                    var patternLSB = BitOperations.GetLSB(normalPattern[i]);
                    normalPattern[i] = BitOperations.PopLSB(normalPattern[i]);

                    expandedPattern[i] |= GetPattern(patternLSB);
                }

                expandedPattern[i] &= ~(1ul << i);
            }

            return expandedPattern;
        }

        /// <summary>
        /// Calculates pattern for the specified field.
        /// </summary>
        /// <param name="fieldBitboard">The field bitboard.</param>
        /// <returns>The pattern for the specified field.</returns>
        private ulong GetPattern(ulong fieldBitboard)
        {
            return ((fieldBitboard & ~BitConstants.AFile) << 1) |
                   ((fieldBitboard & ~BitConstants.HFile) << 7) |
                    (fieldBitboard << 8) |
                   ((fieldBitboard & ~BitConstants.AFile) << 9) |
                   ((fieldBitboard & ~BitConstants.HFile) >> 1) |
                   ((fieldBitboard & ~BitConstants.AFile) >> 7) |
                    (fieldBitboard >> 8) |
                   ((fieldBitboard & ~BitConstants.HFile) >> 9);
        }
    }
}
