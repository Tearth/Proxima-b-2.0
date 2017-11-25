using Proxima.Core.Commons.BitHelpers;

namespace Proxima.Core.MoveGenerators.PatternGenerators
{
    public class KnightPatternGenerator
    {
        public KnightPatternGenerator()
        {
        }

        public ulong[] Generate()
        {
            var predefinedMoves = new ulong[64];
            var bitPosition = 1ul;

            for (int i = 0; i < 64; i++)
            {
                predefinedMoves[i] = GetPattern(bitPosition);
                bitPosition <<= 1;
            }

            return predefinedMoves;
        }

        private ulong GetPattern(ulong field)
        {
            return ((field & ~BitConstants.AFile) << 17) |
                   ((field & ~BitConstants.HFile) << 15) |
                   ((field & ~BitConstants.AFile & ~BitConstants.BFile) << 10) |
                   ((field & ~BitConstants.GFile & ~BitConstants.HFile) << 6) |
                   ((field & ~BitConstants.AFile & ~BitConstants.BFile) >> 6) |
                   ((field & ~BitConstants.GFile & ~BitConstants.HFile) >> 10) |
                   ((field & ~BitConstants.AFile) >> 15) |
                   ((field & ~BitConstants.HFile) >> 17);
        }
    }
}
