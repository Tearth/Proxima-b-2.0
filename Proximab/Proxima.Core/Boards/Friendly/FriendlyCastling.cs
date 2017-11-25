using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyCastling
    {
        public bool WhiteShortCastlingPossibility { get; set; }
        public bool WhiteLongCastlingPossibility { get; set; }
        public bool BlackShortCastlingPossibility { get; set; }
        public bool BlackLongCastlingPossibility { get; set; }

        public bool WhiteCastlingDone { get; set; }
        public bool BlackCastlingDone { get; set; }

        public FriendlyCastling()
        {
        }

        public FriendlyCastling(bool whiteShortPossibility, bool whiteLongPossibility,
                                bool blackShortPossibility, bool blackLongPossibility,
                                bool whiteDone, bool blackDone)
        {
            WhiteShortCastlingPossibility = whiteShortPossibility;
            WhiteLongCastlingPossibility = whiteLongPossibility;
            BlackShortCastlingPossibility = blackShortPossibility;
            BlackLongCastlingPossibility = blackLongPossibility;

            WhiteCastlingDone = whiteDone;
            BlackCastlingDone = blackDone;
        }

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
