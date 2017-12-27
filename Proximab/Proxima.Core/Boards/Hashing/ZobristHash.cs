using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Boards.Hashing
{
    /// <summary>
    /// Represents a set of methods to calculating Zobrist hash.
    /// </summary>
    public static class ZobristHash
    {
        /// <summary>
        /// Calculates Zobrist hash for the specified parameters.
        /// </summary>
        /// <param name="pieces">The pieces array.</param>
        /// <param name="castling">The castling flags array.</param>
        /// <param name="enPassant">The en passant array.</param>
        /// <returns>The Zobrist hash.</returns>
        public static ulong Calculate(ulong[] pieces, bool[] castling, ulong[] enPassant)
        {
            var hash = 0ul;

            hash = CalculatePieces(hash, pieces);
            hash = CalculateCastling(hash, castling);
            hash = CalculateEnPassant(hash, enPassant);

            return hash;
        }

        /// <summary>
        /// Calclates Zobrist hash for pieces.
        /// </summary>
        /// <param name="hash">The curent hash.</param>
        /// <param name="pieces">The array of pieces.</param>
        /// <returns>The updated Zobrist hash.</returns>
        private static ulong CalculatePieces(ulong hash, ulong[] pieces)
        {
            for (int colorIndex = 0; colorIndex < 2; colorIndex++)
            {
                var color = (Color)colorIndex;
                for (int pieceIndex = 0; pieceIndex < 6; pieceIndex++)
                {
                    var piece = (PieceType)pieceIndex;
                    var piecesArray = pieces[FastArray.GetPieceIndex(color, piece)];

                    while (piecesArray != 0)
                    {
                        var pieceLSB = BitOperations.GetLSB(piecesArray);
                        piecesArray = BitOperations.PopLSB(piecesArray);

                        var fieldIndex = BitOperations.GetBitIndex(pieceLSB);

                        hash ^= ZobristContainer.Pieces[FastArray.GetZobristPieceIndex(color, piece, fieldIndex)];
                    }
                }
            }

            return hash;
        }

        /// <summary>
        /// Calclates Zobrist hash for castling flags.
        /// </summary>
        /// <param name="hash">The curent hash.</param>
        /// <param name="castling">The array of castling flags.</param>
        /// <returns>The updated Zobrist hash.</returns>
        private static ulong CalculateCastling(ulong hash, bool[] castling)
        {
            for (int i = 0; i < 4; i++)
            {
                if (castling[i])
                {
                    hash ^= ZobristContainer.Castling[i];
                }
            }

            return hash;
        }

        /// <summary>
        /// Calclates Zobrist hash for en passant.
        /// </summary>
        /// <param name="hash">The curent hash.</param>
        /// <param name="enPassant">The array of en passant.</param>
        /// <returns>The updated Zobrist hash.</returns>
        private static ulong CalculateEnPassant(ulong hash, ulong[] enPassant)
        {
            for (int colorIndex = 0; colorIndex < 2; colorIndex++)
            {
                var enPassantToParse = enPassant[colorIndex];

                while (enPassantToParse != 0)
                {
                    var pieceLSB = BitOperations.GetLSB(enPassantToParse);
                    enPassantToParse = BitOperations.PopLSB(enPassantToParse);

                    var fieldIndex = BitOperations.GetBitIndex(pieceLSB);
                    var fieldPosition = BitPositionConverter.ToPosition(fieldIndex);

                    hash ^= ZobristContainer.EnPassant[fieldPosition.X - 1];
                }
            }

            return hash;
        }
    }
}
