using System;

namespace Proxima.Core.MoveGenerators
{
    [Flags]
    public enum GeneratorMode
    {
        CalculateMoves = 1,
        CalculateAttacks = 2,
    }
}
