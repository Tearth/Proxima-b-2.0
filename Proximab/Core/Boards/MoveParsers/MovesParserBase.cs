using Core.Commons;
using Core.Commons.Moves;

namespace Core.Boards.MoveParsers
{
    public abstract class MovesParserBase
    {
        protected MoveType GetMoveType(ulong patternLSB, ulong enemyOccupation)
        {
            if ((patternLSB & enemyOccupation) != 0)
            {
                return MoveType.Kill;
            }

            return MoveType.Quiet;
        }
    }
}
