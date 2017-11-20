namespace GUI.ContentDefinitions.Commands
{
    /// <summary>
    /// Represents information about the command argument definition
    /// </summary>
    public class CommandArgumentDefinition
    {
        /// <summary>
        /// Gets or sets the type (string/int/bool/etc.).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the description of the command argument.
        /// </summary>
        public string Description { get; set; }
    }
}
