using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Evaluation.Material
{
    /// <summary>
    /// Represents a set of methods to operate on incremental material evaluation results.
    /// </summary>
    public static class IncrementalMaterial
    {
        /// <summary>
        /// Calculates a new material evaluation result based on the value of the new piece.
        /// </summary>
        /// <param name="material">The current material evaluation result.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="color">The piece color.</param>
        /// <returns>The updated material evaluation result.</returns>
        public static int AddPiece(int material, PieceType pieceType, Color color)
        {
            var pieceValue = MaterialValues.PieceValues[(int)pieceType];

            switch (color)
            {
                case Color.White: return material + pieceValue;
                case Color.Black: return material - pieceValue;
            }

            return 0;
        }

        /// <summary>
        /// Calculates a new material evaluation result based on the value of the removed piece.
        /// </summary>
        /// <param name="material">The current material evaluation result.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="color">The piece color.</param>
        /// <returns>The updated material evaluation result.</returns>
        public static int RemovePiece(int material, PieceType pieceType, Color color)
        {
            var pieceValue = MaterialValues.PieceValues[(int)pieceType];

            switch (color)
            {
                case Color.White: return material - pieceValue;
                case Color.Black: return material + pieceValue;
            }

            return 0;
        }
    }
}
