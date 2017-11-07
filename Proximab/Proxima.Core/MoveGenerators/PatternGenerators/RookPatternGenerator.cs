using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.MoveGenerators.PatternGenerators
{
    public class RookPatternGenerator
    {
        public ulong[] Generate()
        {
            var predefinedMoves = new ulong[64];

            for(int i=0; i<64; i++)
            {
                var fieldBit = 1ul << i;
                var position = BitPositionConverter.ToPosition(i);

                var filePattern = GetFilePattern(position) & ~BitConstants.TopBottomEdge;
                var rankPattern = GetRankPattern(position) & ~BitConstants.RightLeftEdge;

                predefinedMoves[i] = ~fieldBit & (filePattern | rankPattern);
            }

            return predefinedMoves;
        }

        ulong GetFilePattern(Position position)
        {
            return BitConstants.AFile >> (position.X - 1);
        }

        ulong GetRankPattern(Position position)
        {
            return BitConstants.ARank << ((position.Y - 1) << 3);
        }
    }
}
