using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Boards.Hashing
{
    /// <summary>
    /// Represents a set of methods to operate on incremental Zobrist hash.
    /// </summary>
    public static class IncrementalZobrist
    {
        /// <summary>
        /// Adds or removes piece from Zobrist hash.
        /// </summary>
        /// <param name="color">The piece color.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="field">The field bitboard.</param>
        /// <param name="bitboard">The bitboard.</param>
        public static void AddOrRemovePiece(Color color, PieceType pieceType, ulong field, Bitboard bitboard)
        {
            var fieldIndex = BitOperations.GetBitIndex(field);
            var index = FastArray.GetZobristPieceIndex(color, pieceType, fieldIndex);

            bitboard.Hash ^= ZobristContainer.Pieces[index];
        }

        /// <summary>
        /// Removes castling possibility Zobrist hash.
        /// </summary>
        /// <param name="color">The piece color.</param>
        /// <param name="castlingType">The castling type.</param>
        /// <param name="bitboard">The bitboard.</param>
        public static void RemoveCastlingPossibility(Color color, CastlingType castlingType, Bitboard bitboard)
        {
            var castlingIndex = FastArray.GetCastlingIndex(color, castlingType);

            if (bitboard.CastlingPossibility[castlingIndex])
            {
                bitboard.Hash ^= ZobristContainer.Castling[castlingIndex];
            }
        }

        /// <summary>
        /// Adds en passant to Zobrist hash.
        /// </summary>
        /// <param name="color">The piece color.</param>
        /// <param name="field">The field bitboard.</param>
        /// <param name="bitboard">The bitboard.</param>
        public static void AddEnPassant(Color color, ulong field, Bitboard bitboard)
        {
            var fieldIndex = BitOperations.GetBitIndex(field);
            var fieldPosition = BitPositionConverter.ToPosition(fieldIndex);

            bitboard.Hash ^= ZobristContainer.EnPassant[fieldPosition.X - 1];
        }

        /// <summary>
        /// Clears en passant from Zobrist hash.
        /// </summary>
        /// <param name="color">The piece color.</param>
        /// <param name="bitboard">The bitboard.</param>
        public static void ClearEnPassant(Color color, Bitboard bitboard)
        {
            var enPassantToParse = bitboard.EnPassant[(int)color];

            while (enPassantToParse != 0)
            {
                var fieldLSB = BitOperations.GetLSB(enPassantToParse);
                enPassantToParse = BitOperations.PopLSB(enPassantToParse);

                var fieldIndex = BitOperations.GetBitIndex(fieldLSB);
                var fieldPosition = BitPositionConverter.ToPosition(fieldIndex);

                bitboard.Hash ^= ZobristContainer.EnPassant[fieldPosition.X - 1];
            }
        }
    }
}
