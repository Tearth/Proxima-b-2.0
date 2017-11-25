using Proxima.Core.Commons.Exceptions;

namespace Proxima.Core.Commons.Pieces
{
    /// <summary>
    /// Represents a set of methods to converting between pieces and their symbols.
    /// </summary>
    public static class PieceConverter
    {
        public const char PawnSymbol = 'P';
        public const char RookSymbol = 'R';
        public const char KnightSymbol = 'N';
        public const char BishopSymbol = 'B';
        public const char QueenSymbol = 'Q';
        public const char KingSymbol = 'K';

        /// <summary>
        /// Calculates the symbol for the specified piece.
        /// </summary>
        /// <param name="piece">The piece type.</param>
        /// <exception cref="PieceSymbolNotFoundException">Thrown when the specified piece type is not recognized.</exception>
        /// <returns>The piece symbol.</returns>
        public static char GetSymbol(PieceType piece)
        {
            switch (piece)
            {
                case PieceType.Pawn: return PawnSymbol;
                case PieceType.Rook: return RookSymbol;
                case PieceType.Knight: return KnightSymbol;
                case PieceType.Bishop: return BishopSymbol;
                case PieceType.Queen: return QueenSymbol;
                case PieceType.King: return KingSymbol;
            }

            throw new PieceSymbolNotFoundException();
        }

        /// <summary>
        /// Calculates the piece type basing on the piece symbol.
        /// </summary>
        /// <param name="piece">The piece symbol.</param>
        /// <exception cref="PieceSymbolNotFoundException">Thrown when the specified piece symbol is not recognized.</exception>
        /// <returns>The piece type.</returns>
        public static PieceType GetPiece(char pieceSymbol)
        {
            switch (pieceSymbol)
            {
                case PawnSymbol: return PieceType.Pawn;
                case RookSymbol: return PieceType.Rook;
                case KnightSymbol: return PieceType.Knight;
                case BishopSymbol: return PieceType.Bishop;
                case QueenSymbol: return PieceType.Queen;
                case KingSymbol: return PieceType.King;
            }

            throw new PieceSymbolNotFoundException();
        }
    }
}
