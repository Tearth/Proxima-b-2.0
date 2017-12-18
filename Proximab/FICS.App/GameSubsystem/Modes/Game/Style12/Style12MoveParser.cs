using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace FICS.App.GameSubsystem.Modes.Game.Style12
{
    /// <summary>
    /// Represents a set of methods to parse Style12 moves received from FICS.
    /// </summary>
    public class Style12MoveParser
    {
        private const string ShortCastlingNotation = "o-o";
        private const string LongCastlingNotation = "o-o-o";

        private const int MoveNotationLength = 7;
        private const int MoveNotationWithPromotionLength = 9;

        private readonly Position _initialWhiteKingPosition = new Position(5, 1);
        private readonly Position _whiteKingPositionAfterShortCastling = new Position(7, 1);
        private readonly Position _whiteKingPositionAfterLongCastling = new Position(3, 1);

        private readonly Position _initialBlackKingPosition = new Position(5, 8);
        private readonly Position _blackKingPositionAfterShortCastling = new Position(7, 8);
        private readonly Position _blackKingPositionAfterLongCastling = new Position(3, 8);

        /// <summary>
        /// Parses FICS response to Style12 move object.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="color">The color of the player making the move.</param>
        /// <returns>The Style12 move object. If passed text parameter is invalid, returns null.</returns>
        public Style12Move Parse(string text, Color color)
        {
            if (text == ShortCastlingNotation)
            {
                return ParseShortCastling(color);
            }
            else if (text == LongCastlingNotation)
            {
                return ParseLongCastling(color);
            }
            else if (text.Length == MoveNotationLength)
            {
                return ParseMove(text, color);
            }
            else if (text.Length == MoveNotationWithPromotionLength)
            {
                return ParsePromotionMove(text, color);
            }

            return null;
        }

        /// <summary>
        /// Parses short castling.
        /// </summary>
        /// <param name="color">The color of the player making the move.</param>
        /// <returns>The Style12 move object. If passed text parameter is invalid, returns null.</returns>
        private Style12Move ParseShortCastling(Color color)
        {
            var fromPosition = color == Color.White ? _initialWhiteKingPosition : _initialBlackKingPosition;
            var toPosition = color == Color.White ? _whiteKingPositionAfterShortCastling : _blackKingPositionAfterShortCastling;

            return new Style12Move(PieceType.King, fromPosition, toPosition);
        }

        /// <summary>
        /// Parses long castling.
        /// </summary>
        /// <param name="color">The color of the player making the move.</param>
        /// <returns>The Style12 move object. If passed text parameter is invalid, returns null.</returns>
        private Style12Move ParseLongCastling(Color color)
        {
            var fromPosition = color == Color.White ? _initialWhiteKingPosition : _initialBlackKingPosition;
            var toPosition = color == Color.White ? _whiteKingPositionAfterLongCastling : _blackKingPositionAfterLongCastling;

            return new Style12Move(PieceType.King, fromPosition, toPosition);
        }

        /// <summary>
        /// Parses simple move.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="color">The color of the player making the move.</param>
        /// <returns>The Style12 move object. If passed text parameter is invalid, returns null.</returns>
        private Style12Move ParseMove(string text, Color color)
        {
            var pieceType = GetPieceType(text);
            var fromPosition = GetSourcePiecePosition(text);
            var toPosition = GetDestinationPiecePosition(text);

            return new Style12Move(pieceType, fromPosition, toPosition);
        }

        /// <summary>
        /// Parses promotion move.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="color">The color of the player making the move.</param>
        /// <returns>The Style12 move object. If passed text parameter is invalid, returns null.</returns>
        private Style12Move ParsePromotionMove(string text, Color color)
        {
            var pieceType = GetPieceType(text);
            var fromPosition = GetSourcePiecePosition(text);
            var toPosition = GetDestinationPiecePosition(text);
            var promotionPieceType = GetPromotionPieceType(text);

            return new Style12Move(pieceType, fromPosition, toPosition, promotionPieceType);
        }

        /// <summary>
        /// Gets the piece type by parsing the Style12 move.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>The piece type.</returns>
        private PieceType GetPieceType(string text)
        {
            var pieceSymbol = text[0];
            return PieceConverter.GetPiece(pieceSymbol);
        }

        /// <summary>
        /// Gets the promotion piece type by parsing the Style12 move.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>The promotion piece type.</returns>
        private PieceType GetPromotionPieceType(string text)
        {
            var pieceSymbol = text[8];
            return PieceConverter.GetPiece(pieceSymbol);
        }

        /// <summary>
        /// Gets the source piece position by parsing the Style12 move.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>The source piece position.</returns>
        private Position GetSourcePiecePosition(string text)
        {
            var positionIndex = 2;
            var positionLength = 2;

            var fromSubstring = text.Substring(positionIndex, positionLength);
            return PositionConverter.ToPosition(fromSubstring);
        }

        /// <summary>
        /// Gets the destination piece position by parsing the Style12 move.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>The destination piece position.</returns>
        private Position GetDestinationPiecePosition(string text)
        {
            var positionIndex = 5;
            var positionLength = 2;

            var toSubstring = text.Substring(positionIndex, positionLength);
            return PositionConverter.ToPosition(toSubstring);
        }
    }
}
