namespace GUI.App.Source.CommandsSubsystem
{
    /// <summary>
    /// Represents a command handle.
    /// </summary>
    internal class CommandHandle
    {
        /// <summary>
        /// Gets or sets the command type.
        /// </summary>
        public CommandType CommandType { get; private set; }

        /// <summary>
        /// Gets or sets the command group.
        /// </summary>
        public CommandGroup CommandGroup { get; private set; }

        /// <summary>
        /// Gets or sets the execute command delegate.
        /// </summary>
        public ExecuteCommandDelegate ExecuteCommandDelegate { get; private set; }

        public CommandHandle(CommandType commandType, CommandGroup commandGroup, ExecuteCommandDelegate executeCommandDelegate)
        {
            CommandType = commandType;
            CommandGroup = commandGroup;
            ExecuteCommandDelegate = executeCommandDelegate;
        }
    }
}
