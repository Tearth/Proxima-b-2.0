using System;

namespace Proxima.Core.MoveGenerators
{
    /// <summary>
    /// Represents the generator modes.
    /// </summary>
    [Flags]
    public enum GeneratorMode
    {
        CalculateMoves = 1,
        CalculateAttacks = 2
    }
}
