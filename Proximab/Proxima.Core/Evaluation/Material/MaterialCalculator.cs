﻿using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Evaluation.Material
{
    /// <summary>
    /// Represents a set of methods to evaluate material.
    /// </summary>
    /// <remarks>
    /// Material is one of the most important aspects of the chess and has the highest ratio from all
    /// evaluation calculators. Every piece has a specified value and sum of these will show which player
    /// has a significant adventage.
    /// </remarks>
    public class MaterialCalculator
    {
        /// <summary>
        /// Calculates a material evaluation result.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The material evaluation result.</returns>
        public int Calculate(Bitboard bitboard)
        {
            var whiteMaterial = GetMaterialValue(Color.White, bitboard);
            var blackMaterial = GetMaterialValue(Color.Black, bitboard);

            return whiteMaterial - blackMaterial;
        }

        /// <summary>
        /// Calculates a detailed material evaluation result.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The detailed material evaluation result.</returns>
        public MaterialData CalculateDetailed(Bitboard bitboard)
        {
            return new MaterialData
            {
                WhiteMaterial = GetMaterialValue(Color.White, bitboard),
                BlackMaterial = GetMaterialValue(Color.Black, bitboard)
            };
        }

        /// <summary>
        /// Calculates a material evaluation result for the specified player by adding all piece values and
        /// multiplying them by the specified ratio.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The material evaluation result for the specified player.</returns>
        private int GetMaterialValue(Color color, Bitboard bitboard)
        {
            var material = 0;

            for (int piece = 0; piece < 6; piece++)
            {
                var piecesToParse = bitboard.Pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];
                var piecesCount = BitOperations.Count(piecesToParse);
                
                material += piecesCount * MaterialValues.PieceValues[piece];
            }

            return material;
        }
    }
}
