using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.Moves;

namespace CECP.App.GameSubsystem.Modes.Game.Moves
{
    public class CECPMoveParser
    {
        public CECPMove Parse(string moveText)
        {
            if (moveText.Length == 4)
            {
                return ParseNormalMove(moveText);
            }
            else if (moveText.Length == 5)
            {
                return ParsePromotionMove(moveText);
            }

            return null;
        }

        private CECPMove ParseNormalMove(string moveText)
        {
            var fromText = moveText.Substring(0, 2);
            var toText = moveText.Substring(2, 2);

            var fromPosition = PositionConverter.ToPosition(fromText);
            var toPosition = PositionConverter.ToPosition(toText);

            return new CECPMove(fromPosition, toPosition);
        }

        private CECPMove ParsePromotionMove(string moveText)
        {
            var fromText = moveText.Substring(0, 2);
            var toText = moveText.Substring(2, 2);
            var promotionPieceSymbol = Convert.ToChar(moveText.Substring(4, 1));

            var fromPosition = PositionConverter.ToPosition(fromText);
            var toPosition = PositionConverter.ToPosition(toText);
            var promotionPieceType = PieceConverter.GetPiece(promotionPieceSymbol);

            return new CECPMove(fromPosition, toPosition, promotionPieceType);
        }
    }
}
