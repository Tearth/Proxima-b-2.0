namespace Core.Boards
{
    public static class BitConstants
    {
        public const ulong FullFill = 0xFFFFFFFFFFFFFFFFul;

        public const ulong ALine = 0x8080808080808080ul;
        public const ulong BLine = 0x4040404040404040ul;
        public const ulong CLine = 0x2020202020202020ul;
        public const ulong DLine = 0x1010101010101010ul;
        public const ulong ELine = 0x0808080808080808ul;
        public const ulong FLine = 0x0404040404040404ul;
        public const ulong GLine = 0x0202020202020202ul;
        public const ulong HLine = 0x0101010101010101ul;

        public const ulong NotALine = FullFill - ALine;
        public const ulong NotBLine = FullFill - BLine;
        public const ulong NotCLine = FullFill - CLine;
        public const ulong NotDLine = FullFill - DLine;
        public const ulong NotELine = FullFill - ELine;
        public const ulong NotFLine = FullFill - FLine;
        public const ulong NotGLine = FullFill - GLine;
        public const ulong NotHLine = FullFill - HLine;
    }
}
