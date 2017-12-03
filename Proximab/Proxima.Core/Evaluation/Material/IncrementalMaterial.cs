using Proxima.Core.Boards;
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
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The piece color.</param>
        /// <param name="pieceType">The piece type.</param>
        public static void AddPiece(Bitboard bitboard, Color color, PieceType pieceType)
        {
            var pieceValue = MaterialValues.PieceValues[(int)pieceType];

            switch (color)
            {
                case Color.White:
                {
                    bitboard.IncEvaluation.Material += pieceValue;
                    break;
                }

                case Color.Black:
                {
                    bitboard.IncEvaluation.Material -= pieceValue;
                    break;
                }
            }
        }

        /// <summary>
        /// Calculates a new material evaluation result based on the value of the removed piece.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The piece color.</param>
        /// <param name="pieceType">The piece type.</param>
        public static void RemovePiece(Bitboard bitboard, Color color, PieceType pieceType)
        {
            var pieceValue = MaterialValues.PieceValues[(int)pieceType];

            switch (color)
            {
                case Color.White:
                {
                    bitboard.IncEvaluation.Material -= pieceValue;
                    break;
                }

                case Color.Black:
                {
                    bitboard.IncEvaluation.Material += pieceValue;
                    break;
                }
            }
        }
    }
}
