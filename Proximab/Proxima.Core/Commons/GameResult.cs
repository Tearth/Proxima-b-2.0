using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.Commons
{
    /// <summary>
    /// Represents available types of game end result.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public enum GameResult
    {
        WhiteWon,
        BlackWon,
        Draw,
        Aborted
    }
}
