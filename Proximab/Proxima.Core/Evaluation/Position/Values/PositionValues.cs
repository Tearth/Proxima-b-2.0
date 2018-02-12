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
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <returns>The position values array.</returns>
        public static int[] GetValues(Color color, GamePhase gamePhase, PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.Pawn: return PawnValues.GetValues(color, gamePhase);
                case PieceType.Knight: return KnightValues.GetValues(color, gamePhase);
                case PieceType.Bishop: return BishopValues.GetValues(color, gamePhase);
                case PieceType.Rook: return RookValues.GetValues(color, gamePhase);
                case PieceType.Queen: return QueenValues.GetValues(color, gamePhase);
                case PieceType.King: return KingValues.GetValues(color, gamePhase);
            }

            return null;
        }
    }
}
