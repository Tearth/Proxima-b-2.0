namespace Proxima.Core.MoveGenerators
{
    /// <summary>
    /// Represents a container for the castling constants.
    /// </summary>
    public class CastlingConstants
    {
        public const ulong InitialKingBitboard = 0x08;
        public const ulong InitialRightRookBitboard = 0x01;
        public const ulong InitialLeftRookBitboard = 0x80;

        public const ulong ShortCastlingCheckArea = 0x0e;
        public const ulong LongCastlingCheckArea = 0x38;

        public const ulong ShortCastlingMoveArea = 0x06;
        public const ulong LongCastlingMoveArea = 0x70;
    }
}
