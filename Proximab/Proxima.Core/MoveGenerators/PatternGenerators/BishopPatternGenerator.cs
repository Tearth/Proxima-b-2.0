using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;

namespace Proxima.Core.MoveGenerators.PatternGenerators
{
    public class BishopPatternGenerator
    {
        public ulong[] Generate()
        {
            var predefinedMoves = new ulong[64];

            for (int i = 0; i < 64; i++)
            {
                var fieldBit = 1ul << i;
                var upperPattern = GetUpperPattern(fieldBit, 7) | GetUpperPattern(fieldBit, 9);
                var bottomPattern = GetBottomPattern(fieldBit, 7) | GetBottomPattern(fieldBit, 9);

                predefinedMoves[i] = fieldBit | upperPattern | bottomPattern;
            }

            return predefinedMoves;
        }

        public ulong GetUpperPattern(ulong fieldBit, int shift)
        {
            if((fieldBit & BitConstants.BitBoardWithoutEdges) == 0 &&
              ((fieldBit << shift) & BitConstants.BitBoardWithoutEdges) == 0)
                return 0;

            var pattern = 0ul;
            do
            {
                fieldBit <<= shift;
                pattern |= fieldBit;
            }
            while ((fieldBit & BitConstants.BitBoardWithoutEdges) != 0);

            return pattern;
        }

        public ulong GetBottomPattern(ulong fieldBit, int shift)
        {
            if ((fieldBit & BitConstants.BitBoardWithoutEdges) == 0 &&
               ((fieldBit >> shift) & BitConstants.BitBoardWithoutEdges) == 0)
                return 0;

            var pattern = 0ul;
            do
            {
                fieldBit >>= shift;
                pattern |= fieldBit;
            }
            while ((fieldBit & BitConstants.BitBoardWithoutEdges) != 0);

            return pattern;
        }
    }
}
