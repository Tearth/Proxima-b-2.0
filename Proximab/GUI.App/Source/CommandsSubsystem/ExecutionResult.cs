namespace GUI.App.Source.CommandsSubsystem
{
    /// <summary>
    /// Represents command execution results.
    /// </summary>
    internal enum ExecutionResult
    {
        Success,
        EmptyCommand,
        CommandNotFound,
        InvalidCommandFormat
    }
}
