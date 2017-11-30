using Proxima.Core.Boards;
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
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="bitBoard">The bitboard.</param>
        /// <returns>The material evaluation result.</returns>
        public int Calculate(GamePhase gamePhase, BitBoard bitBoard)
        {
            var whiteMaterial = GetMaterialValue(Color.White, gamePhase, bitBoard);
            var blackMaterial = GetMaterialValue(Color.Black, gamePhase, bitBoard);

            return whiteMaterial - blackMaterial;
        }

        /// <summary>
        /// Calculates a detailed material evaluation result.
        /// </summary>
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="bitBoard">The bitboard.</param>
        /// <returns>The detailed material evaluation result.</returns>
        public MaterialData CalculateDetailed(GamePhase gamePhase, BitBoard bitBoard)
        {
            return new MaterialData
            {
                WhiteMaterial = GetMaterialValue(Color.White, gamePhase, bitBoard),
                BlackMaterial = GetMaterialValue(Color.Black, gamePhase, bitBoard)
            };
        }

        /// <summary>
        /// Calculates a material evaluation result for the specified player by adding all piece values and
        /// multiplying them by the specified ratio.
        /// </summary>
        /// <param name="color">The king color.</param>
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="bitBoard">The bitboard.</param>
        /// <returns>The material evaluation result for the specified player.</returns>
        private int GetMaterialValue(Color color, GamePhase gamePhase, BitBoard bitBoard)
        {
            var material = 0;

            for (int piece = 0; piece < 6; piece++)
            {
                var piecesToParse = bitBoard.Pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];
                var piecesCount = BitOperations.Count(piecesToParse);
                
                material += piecesCount * MaterialValues.PieceValues[piece];
            }

            return material;
        }
    }
}
