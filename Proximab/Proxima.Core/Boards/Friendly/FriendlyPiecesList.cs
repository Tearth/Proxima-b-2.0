using System.Collections.Generic;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Boards.Friendly
{
    /// <summary>
    /// Represents a list of pieces in the user-friendly way.
    /// </summary>
    public class FriendlyPiecesList : List<FriendlyPiece>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyPiecesList"/> class.
        /// </summary>
        public FriendlyPiecesList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyPiecesList"/> class.
        /// </summary>
        /// <param name="pieces">The array of pieces.</param>
        public FriendlyPiecesList(ulong[] pieces)
        {
            for (var i = 0; i < 12; i++)
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
