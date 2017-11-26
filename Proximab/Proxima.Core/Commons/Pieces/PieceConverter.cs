using Proxima.Core.Commons.Exceptions;
using Proxima.Core.Helpers.Bidirectional;

namespace Proxima.Core.Commons.Pieces
{
    /// <summary>
    /// Represents a set of methods to converting between pieces and their symbols.
    /// </summary>
    public static class PieceConverter
    {
        static BidirectionalDictionary<PieceType, char> _pieces;

        public static void Init()
        {
            _pieces = new BidirectionalDictionary<PieceType, char>();

            _pieces.Add(PieceType.Pawn, 'P');
            _pieces.Add(PieceType.Rook, 'R');
            _pieces.Add(PieceType.Knight, 'N');
            _pieces.Add(PieceType.Bishop, 'B');
            _pieces.Add(PieceType.Queen, 'Q');
            _pieces.Add(PieceType.King, 'K');
        }

        /// <summary>
        /// Calculates the symbol for the specified piece.
        /// </summary>
        /// <param name="piece">The piece type.</param>
        /// <exception cref="PieceSymbolNotFoundException">Thrown when the specified piece type is not recognized.</exception>
        /// <returns>The piece symbol.</returns>
        public static char GetSymbol(PieceType piece)
        {
            if (!_pieces.Forward.ContainsKey(piece))
            {
                throw new PieceSymbolNotFoundException();
            }

            return _pieces.Forward[piece];
        }

        /// <summary>
        /// Calculates the piece type basing on the piece symbol.
        /// </summary>
        /// <param name="pieceSymbol">The piece symbol.</param>
        /// <exception cref="PieceSymbolNotFoundException">Thrown when the specified piece symbol is not recognized.</exception>
        /// <returns>The piece type.</returns>
        public static PieceType GetPiece(char pieceSymbol)
        {
            if (!_pieces.Reverse.ContainsKey(pieceSymbol))
            {
                throw new PieceSymbolNotFoundException();
            }

            return _pieces.Reverse[pieceSymbol];
        }
    }
}
