namespace Core.Boards
{
    public class CastlingData
    {
        public bool WhiteShortCastlingPossible { get; set; }
        public bool WhiteLongCastlingPossible { get; set; }

        public bool BlackShortCastlingPossible { get; set; }
        public bool BlackLongCastlingPossible { get; set; }

        public CastlingData()
        {
            WhiteShortCastlingPossible = true;
            WhiteLongCastlingPossible = true;

            BlackShortCastlingPossible = true;
            BlackLongCastlingPossible = true;
        }

        public CastlingData(CastlingData castlingData)
        {
            WhiteShortCastlingPossible = castlingData.WhiteShortCastlingPossible;
            WhiteLongCastlingPossible = castlingData.WhiteLongCastlingPossible;

            BlackShortCastlingPossible = castlingData.BlackShortCastlingPossible;
            BlackLongCastlingPossible = castlingData.BlackLongCastlingPossible;
        }
    }
}
