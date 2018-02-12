using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.AI.Transposition
{
    /// <summary>
    /// Represents the type of scores used in transposition table.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public enum ScoreType
    {
        Exact,
        UpperBound,
        LowerBound
    }
}
