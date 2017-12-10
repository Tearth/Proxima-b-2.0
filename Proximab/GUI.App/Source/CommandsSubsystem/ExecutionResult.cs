namespace GUI.App.Source.CommandsSubsystem
{
    /// <summary>
    /// Represents command execution results.
    /// </summary>
    public enum ExecutionResult
    {
        Success,
        EmptyCommand,
        CommandNotFound,
        InvalidCommandFormat
    }
}
