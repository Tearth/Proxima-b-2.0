namespace GUI.App.CommandsSubsystem
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
