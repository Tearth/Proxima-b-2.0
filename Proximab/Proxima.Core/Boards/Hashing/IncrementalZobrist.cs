using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Boards.Hashing
{
    public static class IncrementalZobrist
    {
        public static void AddOrRemovePiece(Color color, PieceType pieceType, ulong field, Bitboard bitboard)
        {
            var fieldIndex = BitOperations.GetBitIndex(field);
            var index = FastArray.GetZobristPieceIndex(color, pieceType, fieldIndex);

            bitboard.Hash ^= ZobristContainer.Pieces[index];
        }

        public static void RemoveCastlingPossibility(Color color, CastlingType castlingType, Bitboard bitboard)
        {
            var castlingIndex = FastArray.GetCastlingIndex(color, castlingType);

            if (bitboard.CastlingPossibility[castlingIndex])
            {
                bitboard.Hash ^= ZobristContainer.Castling[castlingIndex];
            }
        }

        public static void AddEnPassant(Color color, ulong field, Bitboard bitboard)
        {
            var fieldIndex = BitOperations.GetBitIndex(field);
            var fieldPosition = BitPositionConverter.ToPosition(fieldIndex);

            bitboard.Hash ^= ZobristContainer.EnPassant[fieldPosition.X - 1];
        }

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
