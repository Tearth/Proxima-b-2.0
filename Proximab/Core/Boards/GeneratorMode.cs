using System;

namespace Core.Boards
{
    [Flags]
    public enum GeneratorMode
    {
        None = 0,
        CalculateMoves = 1,
        CalculateAttacks = 2,
    }
}
