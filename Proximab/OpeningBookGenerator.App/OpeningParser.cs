using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class OpeningParser
    {
        public OpeningParser()
        {
            ProximaCore.Init();
        }

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

        private Move GetMove(Bitboard bitboard, Color color, string textMove)
        {
            if (textMove == "O-O")
            {
                return GetCastling(bitboard, color, CastlingType.Short);
            }

            if (textMove == "O-O-O")
            {
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

        private Move GetCastling(Bitboard bitboard, Color color, CastlingType castlingType)
        {
            return bitboard.Moves
                .OfType<CastlingMove>()
                .First(p => p.Color == color &&
                            p.CastlingType == castlingType);
        }

        private Move GetPawnMove(Bitboard bitboard, Color color, string textMove)
        {
            var toPosition = PositionConverter.ToPosition(textMove);

            return bitboard.Moves
                .OfType<QuietMove>()
                .First(p => p.Color == color &&
                            p.Piece == PieceType.Pawn &&
                            p.To == toPosition);
        }

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

        private Move GetExpandedPieceMove(Bitboard bitboard, Color color, string textMove)
        {
            var initialFileOrRank = textMove[1];

            if (char.IsDigit(initialFileOrRank))
            {
                return GetExpandedPieceWithRankMove(bitboard, color, textMove);
            }

            return GetExpandedPieceWithFileMove(bitboard, color, textMove);
        }

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

        private Move GetKillMove(Bitboard bitboard, Color color, string textMove)
        {
            var pieceSymbol = textMove[0];

            if (char.IsLower(pieceSymbol))
            {
                return GetPawnKillMove(bitboard, color, textMove);
            }

            return GetPieceKillMove(bitboard, color, textMove);
        }

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
