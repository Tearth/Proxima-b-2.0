using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.FICS.Source.GameSubsystem.Modes.Game.Style12
{
    public class Style12Move
    {
        public PieceType PieceType { get; private set; }
        public Position From { get; private set; }
        public Position To { get; private set; }
        public PieceType? PromotionPieceType { get; private set; }

        public Style12Move(PieceType pieceType, Position from, Position to)
        {
            PieceType = pieceType;
            From = from;
            To = to;
        }

        public Style12Move(PieceType pieceType, Position from, Position to, PieceType promotionPieceType)
            : this(pieceType, from, to)
        {
            PromotionPieceType = promotionPieceType;
        }
    }
}
