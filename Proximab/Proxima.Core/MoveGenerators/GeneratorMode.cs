using System;
using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.MoveGenerators
{
    /// <summary>
    /// Represents the generator modes.
    /// </summary>
    [Flags]
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public enum GeneratorMode
    {
        CalculateMoves = 1,
        CalculateAttacks = 2
    }
}
