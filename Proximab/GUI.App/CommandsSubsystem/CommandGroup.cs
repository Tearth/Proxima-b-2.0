using System.Diagnostics.CodeAnalysis;

namespace GUI.App.CommandsSubsystem
{
    /// <summary>
    /// Represents available command groups.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public enum CommandGroup
    {
        None,
        ConsoleManager,
        GUICore,
        GameMode
    }
}
