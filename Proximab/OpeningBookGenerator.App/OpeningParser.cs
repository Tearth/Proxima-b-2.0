using System.Collections.Generic;
using System.Linq;
using OpeningBookGenerator.App.Exceptions;
using Proxima.Core;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;

namespace OpeningBookGenerator.App
{
    /// <summary>
    /// Represents a set of methods for parsing opening line.
    /// </summary>
    public class OpeningParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpeningParser"/> class.
        /// </summary>
        public OpeningParser()
        {
            ProximaCore.Init();
        }

        /// <summary>
        /// Converts list of text notation moves to list of <see cref="Move"/> objects.
        /// </summary>
        /// <param name="moves">The list of text notation moves/</param>
        /// <returns>The list of moves readable for engine.</returns>
        public List<Move> ParseMoves(List<string> moves)
        {
            var parsedMoves = new List<Move>();

            var bitboard = new Bitboard(new DefaultFriendlyBoard());
            var currentColor = Color.White;

            foreach (var move in moves)
            {
                bitboard.Calculate(GeneratorMode.CalculateMoves, false);

                var parsedMove = GetMove(bitboard, currentColor, move);
                bitboard = bitboard.Move(parsedMove);
                parsedMoves.Add(parsedMove);

                currentColor = ColorOperations.Invert(currentColor);
            }

            return parsedMoves;
        }

        /// <summary>
        /// Converts text notation move to <see cref="Move"/> object (e4 = from=e2, to=e4)
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The current color.</param>
        /// <param name="textMove">The text notation to parse.</param>
        /// <returns>The <see cref="Move"/> representation of passed text notation.</returns>
        private Move GetMove(Bitboard bitboard, Color color, string textMove)
        {
            switch (textMove)
            {
                case "O-O":
                    return GetCastling(bitboard, color, CastlingType.Short);
                case "O-O-O":
                    return GetCastling(bitboard, color, CastlingType.Long);
            }

            switch (textMove.Length)
            {
                case 2: return GetPawnMove(bitboard, color, textMove);
                case 3: return GetPieceMove(bitboard, color, textMove);
                case 4 when textMove[1] != 'x': return GetExpandedPieceMove(bitboard, color, textMove);
                case 4 when textMove[1] == 'x': return GetKillMove(bitboard, color, textMove);
            }

            throw new InvalidMoveNotationException();
        }

        /// <summary>
        /// Gets castling move from passed bitboard with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The current color.</param>
        /// <param name="castlingType">The castling type.</param>
        /// <returns>The castling move.</returns>
        private Move GetCastling(Bitboard bitboard, Color color, CastlingType castlingType)
        {
            return bitboard.Moves
                .OfType<CastlingMove>()
                .First(p => p.Color == color &&
                            p.CastlingType == castlingType);
        }

        /// <summary>
        /// Gets the pawn move from passed bitboard with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The current color.</param>
        /// <param name="textMove">The pawn move in text notation.</param>
        /// <returns>The pawn move.</returns>
        private Move GetPawnMove(Bitboard bitboard, Color color, string textMove)
        {
            var toPosition = PositionConverter.ToPosition(textMove);

            return bitboard.Moves
                .OfType<QuietMove>()
                .First(p => p.Color == color &&
                            p.Piece == PieceType.Pawn &&
                            p.To == toPosition);
        }

        /// <summary>
        /// Gets the piece move (not pawn!) from passed bitboard with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The current color.</param>
        /// <param name="textMove">The piece move in text notation.</param>
        /// <returns>The piece move.</returns>
        private Move GetPieceMove(Bitboard bitboard, Color color, string textMove)
        {
            var pieceSymbol = textMove[0];
            var pieceType = PieceConverter.GetPiece(pieceSymbol);

            var toPositionText = textMove.Substring(1, 2);
            var toPosition = PositionConverter.ToPosition(toPositionText);

            return bitboard.Moves
                .OfType<QuietMove>()
                .First(p => p.Color == color &&
                            p.Piece == pieceType &&
                            p.To == toPosition);
        }

        /// <summary>
        /// Gets the expanded (with additional symbol which can means file or rank, eg. Rab3) piece move from
        /// passed bitboard with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The current color.</param>
        /// <param name="textMove">The piece move in text notation.</param>
        /// <returns>The piece move.</returns>
        private Move GetExpandedPieceMove(Bitboard bitboard, Color color, string textMove)
        {
            var initialFileOrRank = textMove[1];

            return char.IsDigit(initialFileOrRank) ?
                GetExpandedPieceWithRankMove(bitboard, color, textMove) :
                GetExpandedPieceWithFileMove(bitboard, color, textMove);
        }

        /// <summary>
        /// Gets the expanded (with additional symbol which means file) piece move from
        /// passed bitboard with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The current color.</param>
        /// <param name="textMove">The piece move in text notation.</param>
        /// <returns>The piece move.</returns>
        private Move GetExpandedPieceWithFileMove(Bitboard bitboard, Color color, string textMove)
        {
            var pieceSymbol = textMove[0];
            var pieceType = PieceConverter.GetPiece(pieceSymbol);

            var initialFile = textMove[1] - 'a' + 1;

            var toPositionText = textMove.Substring(2, 2);
            var toPosition = PositionConverter.ToPosition(toPositionText);

            return bitboard.Moves
                .OfType<QuietMove>()
                .First(p => p.Color == color &&
                            p.Piece == pieceType &&
                            p.From.X == initialFile &&
                            p.To == toPosition);
        }

        /// <summary>
        /// Gets the expanded (with additional symbol which means rank) piece move from
        /// passed bitboard with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The current color.</param>
        /// <param name="textMove">The piece move in text notation.</param>
        /// <returns>The piece move.</returns>
        private Move GetExpandedPieceWithRankMove(Bitboard bitboard, Color color, string textMove)
        {
            var pieceSymbol = textMove[0];
            var pieceType = PieceConverter.GetPiece(pieceSymbol);

            var initialRank = textMove[1] - '1' + 1;

            var toPositionText = textMove.Substring(2, 2);
            var toPosition = PositionConverter.ToPosition(toPositionText);

            return bitboard.Moves
                .OfType<QuietMove>()
                .First(p => p.Color == color &&
                            p.Piece == pieceType &&
                            p.From.Y == initialRank &&
                            p.To == toPosition);
        }

        /// <summary>
        /// Gets the kill move from passed bitboard with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The current color.</param>
        /// <param name="textMove">The piece move in text notation.</param>
        /// <returns>The kill move.</returns>
        private Move GetKillMove(Bitboard bitboard, Color color, string textMove)
        {
            var pieceSymbol = textMove[0];

            return char.IsLower(pieceSymbol) ?
                GetPawnKillMove(bitboard, color, textMove) :
                GetPieceKillMove(bitboard, color, textMove);
        }

        /// <summary>
        /// Gets the pawn kill move from passed bitboard with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The current color.</param>
        /// <param name="textMove">The piece move in text notation.</param>
        /// <returns>The pawn kill move.</returns>
        private Move GetPawnKillMove(Bitboard bitboard, Color color, string textMove)
        {
            var initialRank = textMove[0] - 'a' + 1;

            var toPositionText = textMove.Substring(2, 2);
            var toPosition = PositionConverter.ToPosition(toPositionText);

            return bitboard.Moves
                .Where(p => p is KillMove || p is EnPassantMove)
                .First(p => p.Color == color &&
                            p.Piece == PieceType.Pawn &&
                            p.From.X == initialRank &&
                            p.To == toPosition);
        }

        /// <summary>
        /// Gets the piece kill move from passed bitboard with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="color">The current color.</param>
        /// <param name="textMove">The piece move in text notation.</param>
        /// <returns>The piece kill move.</returns>
        private Move GetPieceKillMove(Bitboard bitboard, Color color, string textMove)
        {
            var pieceSymbol = textMove[0];
            var pieceType = PieceConverter.GetPiece(pieceSymbol);

            var toPositionText = textMove.Substring(2, 2);
            var toPosition = PositionConverter.ToPosition(toPositionText);

            return bitboard.Moves
                .OfType<KillMove>()
                .First(p => p.Color == color &&
                            p.Piece == pieceType &&
                            p.To == toPosition);
        }
    }
}
