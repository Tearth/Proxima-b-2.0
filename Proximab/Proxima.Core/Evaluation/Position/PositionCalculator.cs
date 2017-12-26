using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Evaluation.Position.Values;

namespace Proxima.Core.Evaluation.Position
{
    /// <summary>
    /// Represents a set of methods to evaluate position.
    /// </summary>
    /// <remarks>
    /// Position is an simply information about all piece localisations. On example, knight at border
    /// of the chess board has less mobility, so it is less valuable. By this parameter, we can force AI to develop
    /// pieces more on center, what is very favourable.
    /// </remarks>
    public class PositionCalculator
    {
        /// <summary>
        /// Calculates a position evaluation result.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The position evaluation result.</returns>
        public int Calculate(Bitboard bitboard)
        {
            var whitePosition = GetPosition(bitboard, Color.White);
            var blackPosition = GetPosition(bitboard, Color.Black);

            return whitePosition - blackPosition;
        }

        /// <summary>
        /// Calculates a detailed position evaluation result.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The detailed position evaluation result.</returns>
        public PositionData CalculateDetailed(Bitboard bitboard)
        {
            return new PositionData()
            {
                WhitePosition = GetPosition(bitboard, Color.White),
                BlackPosition = GetPosition(bitboard, Color.Black)
            };
        }

        /// <summary>
        /// Calculates a position evaluation result for the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The player color.</param>
        /// <returns>The position evaluation result.</returns>
        private int GetPosition(Bitboard bitboard, Color color)
        {
            var position = 0;

            for (int piece = 0; piece < 6; piece++)
            {
                var piecesToParse = bitboard.Pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];
                position += GetPositionValue(bitboard, color, (PieceType)piece, piecesToParse);
            }

            return position;
        }

        /// <summary>
        /// Calculates a position evaluation result for the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The player color.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="piecesToParse">The pieces to parse.</param>
        /// <returns>The position evaluation result.</returns>
        private int GetPositionValue(Bitboard bitboard, Color color, PieceType pieceType, ulong piecesToParse)
        {
            var position = 0;
            var array = PositionValues.GetValues(color, pieceType);

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(piecesToParse);
                piecesToParse = BitOperations.PopLSB(piecesToParse);

                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                position += array[FastArray.GetEvaluationValueIndex(bitboard.GamePhase, pieceIndex)];
            }

            return position;
        }
    }
}
