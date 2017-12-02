using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Evaluation.Position.Values;

namespace Proxima.Core.Evaluation.Position
{
    /// <summary>
    /// Represents a set of methods to operate on incremental position evaluation results.
    /// </summary>
    public static class IncrementalPosition
    {
        /// <summary>
        /// Calculates a new position evaluation result based on the value of the new piece and its position.
        /// </summary>
        /// <param name="position">The current position evaluation result.</param>
        /// <param name="color">The piece color.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="piece">The piece bit.</param>
        /// <param name="gamePhase">The current game phase.</param>
        /// <returns>The updated position evaluation result.</returns>
        public static int AddPiece(int position, Color color, PieceType pieceType, ulong piece, GamePhase gamePhase)
        {
            var pieceIndex = BitOperations.GetBitIndex(piece);
            var array = PositionValues.GetValues(color, pieceType);
            var delta = array[FastArray.GetEvaluationValueIndex(gamePhase, pieceIndex)];

            switch (color)
            {
                case Color.White: return position + delta;
                case Color.Black: return position - delta;
            }

            return 0;
        }

        /// <summary>
        /// Calculates a new position evaluation result based on the value of the removed piece and its position.
        /// </summary>
        /// <param name="position">The current position evaluation result.</param>
        /// <param name="color">The piece color.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="piece">The piece bit.</param>
        /// <param name="gamePhase">The current game phase.</param>
        /// <returns>The updated position evaluation result.</returns>
        public static int RemovePiece(int position, Color color, PieceType pieceType, ulong piece, GamePhase gamePhase)
        {
            var pieceIndex = BitOperations.GetBitIndex(piece);
            var array = PositionValues.GetValues(color, pieceType);
            var delta = array[FastArray.GetEvaluationValueIndex(gamePhase, pieceIndex)];

            switch (color)
            {
                case Color.White: return position - delta;
                case Color.Black: return position + delta;
            }

            return 0;
        }
    }
}
