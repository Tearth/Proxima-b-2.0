using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Evaluation.Position.Values
{
    /// <summary>
    /// Represents a set of methods to calculating position values array.
    /// </summary>
    public static class PositionValues
    {
        /// <summary>
        /// Calculates a position values array for the specified color and piece type.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <returns>The position values array.</returns>
        public static int[] GetValues(Color color, PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.Pawn: return PawnValues.GetValues(color);
                case PieceType.Knight: return KnightValues.GetValues(color);
                case PieceType.Bishop: return BishopValues.GetValues(color);
                case PieceType.Rook: return RookValues.GetValues(color);
                case PieceType.Queen: return QueenValues.GetValues(color);
                case PieceType.King: return KingValues.GetValues(color);
            }

            return null;
        }
    }
}
