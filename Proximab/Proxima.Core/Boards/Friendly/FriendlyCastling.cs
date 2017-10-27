namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyCastling
    {
        public bool WhiteShortCastling;
        public bool WhiteLongCastling;
        public bool BlackShortCastling;
        public bool BlackLongCastling;

        public FriendlyCastling()
        {

        }

        public FriendlyCastling(bool whiteShort, bool whiteLong, bool blackShort, bool blackLong)
        {
            WhiteShortCastling = whiteShort;
            WhiteLongCastling = whiteLong;
            BlackShortCastling = blackShort;
            BlackLongCastling = blackLong;
        }

        public FriendlyCastling(bool[] castling)
        {
            WhiteShortCastling = castling[0];
            WhiteLongCastling = castling[1];
            BlackShortCastling = castling[2];
            BlackLongCastling = castling[3];
        }
    }
}
