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

        public void RemoveCastlingPossibility(ref ulong hash, Color color, CastlingType castlingType)
        {
            var index = FastArray.GetCastlingIndex(color, castlingType);

            hash ^= ZobristContainer.Castling[index];
        }

        public void AddEnPassant(ref ulong hash, Color color, ulong field)
        {
            var fieldIndex = BitOperations.GetBitIndex(field);
            var fieldPosition = BitPositionConverter.ToPosition(fieldIndex);

            hash ^= ZobristContainer.EnPassant[fieldPosition.X - 1];
        }
    }
}
