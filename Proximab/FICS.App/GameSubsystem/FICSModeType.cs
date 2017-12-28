using System.Diagnostics.CodeAnalysis;

namespace FICS.App.GameSubsystem
{
    /// <summary>
    /// Represents a list of FICS mode types.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public enum FICSModeType
    {
        Auth,
        Seek,
        Game
    }
}
