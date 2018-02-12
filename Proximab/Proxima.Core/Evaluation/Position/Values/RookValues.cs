﻿using System.Diagnostics.CodeAnalysis;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Position.Values
{
    /// <summary>
    /// Represents a set of evaluation parameters for rook position.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class RookValues
    {
        public static readonly int[][] Pattern =
        {
            // Regular
            new[] {10,   0,   0,   0,   0,   0,   0,  10,
                   10,   0,   0,   0,   0,   0,   0,  10,
                   10,   0,   0,   0,   0,   0,   0,  10,
                   10,   0,   0,   0,   0,   0,   0,  10,
                    5,   0,   0,   0,   0,   0,   0,   5,
                    5,   0,   0,   0,   0,   0,   0,   5,
                    5,   0,   0,   0,   0,   0,   0,   5,
                    0,   0,   0,   10,  5,  10,   0,   0 },

            // End
            new[] { 5,   5,   5,   5,   5,   5,   5,   5,
                    5,   5,   5,   5,   5,   5,   5,   5,
                    5,   5,   5,   5,   5,   5,   5,   5,
                    5,   5,   5,   5,   5,   5,   5,   5,
                    5,   5,   5,   5,   5,   5,   5,   5,
                    5,   5,   5,   5,   5,   5,   5,   5,
                    5,   5,   5,   5,   5,   5,   5,   5,
                    5,   5,   5,   5,   5,   5,   5,   5 }
        };

        public static readonly int[][] WhiteValues = EvaluationFlipper.CalculateWhiteArray(Pattern);
        public static readonly int[][] BlackValues = EvaluationFlipper.CalculateBlackArray(Pattern);

        /// <summary>
        /// Calculates a position values array for the specified player color.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <param name="gamePhase">The game phase.</param>
        /// <returns>The position values array for the specified color.</returns>
        public static int[] GetValues(Color color, GamePhase gamePhase)
        {
            return color == Color.White ? WhiteValues[(int)gamePhase] : BlackValues[(int)gamePhase];
        }
    }
}
