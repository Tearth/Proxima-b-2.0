namespace Proxima.Core.Commons.BitHelpers
{
    public static class BitConstants
    {
        public const ulong AFile = 0x8080808080808080ul;
        public const ulong BFile = 0x4040404040404040ul;
        public const ulong CFile = 0x2020202020202020ul;
        public const ulong DLine = 0x1010101010101010ul;
        public const ulong ELine = 0x0808080808080808ul;
        public const ulong FFile = 0x0404040404040404ul;
        public const ulong GFile = 0x0202020202020202ul;
        public const ulong HFile = 0x0101010101010101ul;

        public const ulong ARank = 0x00000000000000FFul;
        public const ulong BRank = 0x000000000000FF00ul;
        public const ulong CRank = 0x0000000000FF0000ul;
        public const ulong DRank = 0x00000000FF000000ul;
        public const ulong ERank = 0x000000FF00000000ul;
        public const ulong FRank = 0x0000FF0000000000ul;
        public const ulong GRank = 0x00FF000000000000ul;
        public const ulong HRank = 0xFF00000000000000ul;

        public const ulong BitBoardWithoutEdges = ~AFile & ~HFile & ~ARank & ~HRank;
        public const ulong RightLeftEdge = AFile | HFile;
        public const ulong TopBottomEdge = ARank | HRank;

        public const ulong LeftTopBoardPart     = 0xfefcf8f0e0c08000ul;
        public const ulong LeftBottomBoardPart  = 0x80c0e0f0f8fcfefful;
        public const ulong RightTopBoardPart    = 0x7f3f1f0f07030100ul;
        public const ulong RightBottomBoardPart = 0x0103070f1f3f7ffful;

        public const ulong SmallCenter = 0x0000001818000000ul;
        public const ulong BigCenter   = 0x00003c3c3c3c0000ul;
    }
}
