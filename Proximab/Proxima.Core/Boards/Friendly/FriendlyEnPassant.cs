using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    /// <summary>
    /// Represents a en passant data in the user-friendly way.
    /// </summary>
    public class FriendlyEnPassant
    {
        /// <summary>
        /// Gets or sets the white en passant position.
        /// </summary>
        public Position? WhiteEnPassant { get; set; }

        /// <summary>
        /// Gets or sets the white en passant position.
        /// </summary>
        public Position? BlackEnPassant { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyEnPassant"/> class.
        /// </summary>
        public FriendlyEnPassant()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyEnPassant"/> class.
        /// </summary>
        /// <param name="whiteEnPassant">The white en passant position.</param>
        /// <param name="blackEnPassant">The black en passant position.</param>
        public FriendlyEnPassant(Position whiteEnPassant, Position blackEnPassant)
        {
            WhiteEnPassant = whiteEnPassant;
            BlackEnPassant = blackEnPassant;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyEnPassant"/> class.
        /// </summary>
        /// <param name="enPassant">The array of en passant bitboards.</param>
        public FriendlyEnPassant(ulong[] enPassant)
        {
            WhiteEnPassant = GetEnPassantPosition(enPassant[0]);
            BlackEnPassant = GetEnPassantPosition(enPassant[1]);
        }

        /// <summary>
        /// Gets a en passant position from the specified bitboard.
        /// </summary>
        /// <param name="enPassant">The en passant bitboard.</param>
        /// <returns>The en passant position (null if en passant bitboard is equal to zero)</returns>
        private Position? GetEnPassantPosition(ulong enPassant)
        {
            if (enPassant == 0)
            {
                return null;
            }

            var lsb = BitOperations.GetLsb(enPassant);
            enPassant = BitOperations.PopLsb(enPassant);

            var bitIndex = BitOperations.GetBitIndex(lsb);

            return BitPositionConverter.ToPosition(bitIndex);
        }
    }
}
