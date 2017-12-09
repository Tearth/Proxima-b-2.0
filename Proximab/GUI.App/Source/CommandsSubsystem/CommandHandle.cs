namespace GUI.App.Source.CommandsSubsystem
{
    /// <summary>
    /// Represents a command handle.
    /// </summary>
    internal class CommandHandle
    {
        /// <summary>
        /// Gets the command type.
        /// </summary>
        public CommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the command group.
        /// </summary>
        public CommandGroup CommandGroup { get; private set; }

        /// <summary>
        /// Gets the execute command delegate.
        /// </summary>
        public ExecuteCommandDelegate ExecuteCommandDelegate { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandle"/> class.
        /// </summary>
        /// <param name="commandType">The command type.</param>
        /// <param name="commandGroup">The command group.</param>
        /// <param name="executeCommandDelegate">The execute command delegate.</param>
        public CommandHandle(CommandType commandType, CommandGroup commandGroup, ExecuteCommandDelegate executeCommandDelegate)
        {
            CommandType = commandType;
            CommandGroup = commandGroup;
            ExecuteCommandDelegate = executeCommandDelegate;
        }
    }
}
