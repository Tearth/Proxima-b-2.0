using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Position.Values
{
    public static class PositionValues
    {
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
