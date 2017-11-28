namespace Proxima.Core.MoveGenerators
{
    public class CastlingConstants
    {
        public const ulong KingLSB = 0x8;
        public const ulong RightRookLSB = 0x01;
        public const ulong LeftRookLSB = 0x80;

        public const ulong ShortCastlingCheckArea = 0x0eul;
        public const ulong LongCastlingCheckArea = 0x38ul;

        public const ulong ShortCastlingMoveArea = 0x06ul;
        public const ulong LongCastlingMoveArea = 0x70ul;
    }
}
