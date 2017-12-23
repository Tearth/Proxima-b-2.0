using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace CECP.App.GameSubsystem.Modes.Game
{
    public class CECPMove
    {
        public Position From { get; private set; }
        public Position To { get; private set; }
        public PieceType? PromotionPiece { get; private set; }
        
        public CECPMove(Position from, Position to) : this(from, to, null)
        {
        }

        public CECPMove(Position from, Position to, PieceType? promotionPiece)
        {
            From = from;
            To = to;
            PromotionPiece = promotionPiece;
        }
    }
}
