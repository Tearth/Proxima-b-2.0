using System.Collections.Generic;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyPiecesList : List<FriendlyPiece>
    {
        public FriendlyPiecesList() : base()
        {
        }

        public FriendlyPiecesList(ulong[] pieces) : base()
        {
            for (int i = 0; i < 12; i++)
            {
                var pieceArray = pieces[i];

                while (pieceArray != 0)
                {
                    var lsb = BitOperations.GetLSB(pieceArray);
                    pieceArray = BitOperations.PopLSB(pieceArray);

                    var bitIndex = BitOperations.GetBitIndex(lsb);
                    var position = BitPositionConverter.ToPosition(bitIndex);

                    var pieceType = (PieceType)(i % 6);
                    var pieceColor = (Color)(i / 6);

                    Add(new FriendlyPiece(position, pieceType, pieceColor));
                }
            }
        }
    }
}
