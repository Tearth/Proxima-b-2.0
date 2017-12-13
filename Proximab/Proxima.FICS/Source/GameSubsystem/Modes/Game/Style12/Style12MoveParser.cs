using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.FICS.Source.GameSubsystem.Modes.Game.Style12
{
    public class Style12MoveParser
    {
        public Style12Move Parse(string move, Color color)
        {
            if (move == "o-o")
            {
                return ParseShortCastling(move, color);
            }
            else if (move == "o-o-o")
            {
                return ParseLongCastling(move, color);
            }
            else if (move.Length == 7)
            {
                return ParseMove(move, color);
            }
            else if (move.Length == 9)
            {
                return ParsePromotionMove(move, color);
            }

            return null;
        }

        private Style12Move ParseShortCastling(string move, Color color)
        {
            var fromPosition = color == Color.White ? new Position(5, 1) : new Position(5, 8);
            var toPosition = color == Color.White ? new Position(7, 1) : new Position(7, 8);

            return new Style12Move(PieceType.King, fromPosition, toPosition);
        }

        private Style12Move ParseLongCastling(string move, Color color)
        {
            var fromPosition = color == Color.White ? new Position(5, 1) : new Position(5, 8);
            var toPosition = color == Color.White ? new Position(3, 1) : new Position(3, 8);

            return new Style12Move(PieceType.King, fromPosition, toPosition);
        }

        private Style12Move ParseMove(string move, Color color)
        {
            var pieceType = GetPieceType(move);
            var fromPosition = GetFromPosition(move);
            var toPosition = GetToPosition(move);

            return new Style12Move(pieceType, fromPosition, toPosition);
        }

        private Style12Move ParsePromotionMove(string move, Color color)
        {
            var pieceType = GetPieceType(move);
            var fromPosition = GetFromPosition(move);
            var toPosition = GetToPosition(move);
            var promotionPieceType = GetPromotinoPieceType(move);

            return new Style12Move(pieceType, fromPosition, toPosition, promotionPieceType);
        }

        private PieceType GetPieceType(string move)
        {
            var pieceSymbol = move[0];
            return PieceConverter.GetPiece(pieceSymbol);
        }

        private PieceType GetPromotinoPieceType(string move)
        {
            var pieceSymbol = move[8];
            return PieceConverter.GetPiece(pieceSymbol);
        }

        private Position GetFromPosition(string move)
        {
            var fromSubstring = move.Substring(2, 2);
            return PositionConverter.ToPosition(fromSubstring);
        }

        private Position GetToPosition(string move)
        {
            var toSubstring = move.Substring(5, 2);
            return PositionConverter.ToPosition(toSubstring);
        }
    }
}
