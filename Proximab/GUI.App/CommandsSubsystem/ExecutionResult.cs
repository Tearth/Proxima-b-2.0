using System.Diagnostics.CodeAnalysis;

namespace GUI.App.CommandsSubsystem
{
    /// <summary>
    /// Represents command execution results.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public enum ExecutionResult
    {
        Success,
        EmptyCommand,
        CommandNotFound,
        InvalidCommandFormat
    }
}
