using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Boards.Friendly
{
    /// <summary>
    /// Represents a castling data in the user-friendly way.
    /// </summary>
    public class FriendlyCastling
    {
        /// <summary>
        /// Gets or sets a value indicating whether white short castling is possible.
        /// </summary>
        public bool WhiteShortCastlingPossibility { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether white long castling is possible.
        /// </summary>
        public bool WhiteLongCastlingPossibility { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether black short castling is possible.
        /// </summary>
        public bool BlackShortCastlingPossibility { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether black long castling is possible.
        /// </summary>
        public bool BlackLongCastlingPossibility { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether white castling is done.
        /// </summary>
        public bool WhiteCastlingDone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether black castling is done.
        /// </summary>
        public bool BlackCastlingDone { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyCastling"/> class.
        /// </summary>
        public FriendlyCastling()
        {
            WhiteShortCastlingPossibility = true;
            WhiteLongCastlingPossibility = true;
            BlackShortCastlingPossibility = true;
            BlackLongCastlingPossibility = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyCastling"/> class.
        /// </summary>
        /// <param name="castlingPossibility">The array with castling possibility flags.</param>
        /// <param name="castlingDone">The array with castling done flags.</param>
        public FriendlyCastling(bool[] castlingPossibility, bool[] castlingDone)
        {
            WhiteShortCastlingPossibility = castlingPossibility[0];
            WhiteLongCastlingPossibility = castlingPossibility[1];
            BlackShortCastlingPossibility = castlingPossibility[2];
            BlackLongCastlingPossibility = castlingPossibility[3];

            WhiteCastlingDone = castlingDone[(int)Color.White];
            BlackCastlingDone = castlingDone[(int)Color.Black];
        }
    }
}
