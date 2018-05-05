using System.Diagnostics.CodeAnalysis;

namespace GUI.App.GameSubsystem
{
    /// <summary>
    /// Represents available types of mode.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public enum GameModeType
    {
        Editor,
        AIvsAI,
        PlayervsAI
    }
}
