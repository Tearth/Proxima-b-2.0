using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Randoms;

namespace Proxima.Core.Boards.Hashing
{
    public class ZobristHash
    {
        Random64 _random64;

        public ZobristHash()
        {
            _random64 = new Random64();
        }

        public ulong Calculate(ulong[] pieces, bool[] castling, ulong[] enPassant)
        {
            var hash = 0ul;

            hash = CalculatePieces(hash, pieces);
            hash = CalculateCastling(hash, castling);
            hash = CalculateEnPassant(hash, enPassant);

            return hash;
        }

        ulong CalculatePieces(ulong hash, ulong[] pieces)
        {
            for(int colorIndex=0; colorIndex < 2; colorIndex++)
            {
                var color = (Color)colorIndex;
                for(int pieceIndex=0; pieceIndex < 6; pieceIndex++)
                {
                    var piece = (PieceType)pieceIndex;
                    var piecesArray = pieces[FastArray.GetPieceIndex(color, piece)];

                    while(piecesArray != 0)
                    {
                        var pieceLSB = BitOperations.GetLSB(ref piecesArray);
                        var fieldIndex = BitOperations.GetBitIndex(pieceLSB);

                        hash ^= ZobristContainer.Pieces[FastArray.GetZobristPieceIndex(color, piece, fieldIndex)];
                    }
                }
            }

            return hash;
        }

        ulong CalculateCastling(ulong hash, bool[] castling)
        {
            for(int i=0; i<4; i++)
            {
                if(castling[i])
                {
                    hash ^= ZobristContainer.Castling[i];
                }
            }

            return hash;
        }

        ulong CalculateEnPassant(ulong hash, ulong[] enPassant)
        {
            for(int colorIndex=0; colorIndex < 2; colorIndex++)
            {
                var enPassantToParse = enPassant[colorIndex];

                var pieceLSB = BitOperations.GetLSB(ref enPassantToParse);
                var fieldIndex = BitOperations.GetBitIndex(pieceLSB);
                var fieldPosition = BitPositionConverter.ToPosition(fieldIndex);

                hash ^= ZobristContainer.EnPassant[fieldPosition.X - 1];
            }

            return hash;
        }
    }
}
