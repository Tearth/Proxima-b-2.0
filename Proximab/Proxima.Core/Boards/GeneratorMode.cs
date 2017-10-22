using System;

namespace Proxima.Core.Boards
{
    [Flags]
    public enum GeneratorMode
    {
        CalculateMoves = 1,
        CalculateAttacks = 2,
    }
}
