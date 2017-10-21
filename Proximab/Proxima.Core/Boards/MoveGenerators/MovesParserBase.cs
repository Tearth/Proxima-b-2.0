using Proxima.Core.Commons;
using Proxima.Core.Commons.Moves;

namespace Proxima.Core.Boards.MoveGenerators
{
    public abstract class MovesParserBase
    {
        protected MoveType GetMoveType(ulong patternLSB, ulong enemyOccupancy)
        {
            if ((patternLSB & enemyOccupancy) != 0)
            {
                return MoveType.Kill;
            }

            return MoveType.Quiet;
        }
    }
}
