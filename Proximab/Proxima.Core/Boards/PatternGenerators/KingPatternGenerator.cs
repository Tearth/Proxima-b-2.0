using Proxima.Core.Commons.BitHelpers;

namespace Proxima.Core.Boards.PatternGenerators
{
    public class KingPatternGenerator
    {
        public KingPatternGenerator()
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

        ulong GetPattern(ulong field)
        {
            return ((field & ~BitConstants.AFile) << 1) |
                   ((field & ~BitConstants.HFile) << 7) |
                   ( field                        << 8) |
                   ((field & ~BitConstants.AFile) << 9) |
                   ((field & ~BitConstants.HFile) >> 1) |
                   ((field & ~BitConstants.AFile) >> 7) |
                    (field                        >> 8) |
                   ((field & ~BitConstants.HFile) >> 9);
        }
    }
}
