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
                var position = BitPositionConverter.ToPosition(i);
                var filePattern = GetFilePattern(position);
                var rankPattern = GetRankPattern(position);

                predefinedMoves[i] = filePattern | rankPattern;
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
