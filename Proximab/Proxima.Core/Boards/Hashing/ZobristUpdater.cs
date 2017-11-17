using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Boards.Hashing
{
    public class ZobristUpdater
    {
        public void AddOrRemovePiece(ref ulong hash, Color color, PieceType pieceType, ulong field)
        {
            var fieldIndex = BitOperations.GetBitIndex(field);
            var index = FastArray.GetZobristPieceIndex(color, pieceType, fieldIndex);

            hash ^= ZobristContainer.Pieces[index];
        }

        public void RemoveCastlingPossibility(ref ulong hash, bool[] castling, Color color, CastlingType castlingType)
        {
            var castlingIndex = FastArray.GetCastlingIndex(color, castlingType);

            if (castling[castlingIndex])
            {
                hash ^= ZobristContainer.Castling[castlingIndex];
            }
        }

        public void AddEnPassant(ref ulong hash, Color color, ulong field)
        {
            var fieldIndex = BitOperations.GetBitIndex(field);
            var fieldPosition = BitPositionConverter.ToPosition(fieldIndex);

            hash ^= ZobristContainer.EnPassant[fieldPosition.X - 1];
        }

        public void ClearEnPassant(ref ulong hash, Color color, ulong[] _enPassant)
        {
            var enPassantToParse = _enPassant[(int)color];

            while(enPassantToParse != 0)
            {
                var fieldLSB = BitOperations.GetLSB(enPassantToParse);
                enPassantToParse = BitOperations.PopLSB(enPassantToParse);

                var fieldIndex = BitOperations.GetBitIndex(fieldLSB);
                var fieldPosition = BitPositionConverter.ToPosition(fieldIndex);

                hash ^= ZobristContainer.EnPassant[fieldPosition.X - 1];
            }
        }
    }
}
