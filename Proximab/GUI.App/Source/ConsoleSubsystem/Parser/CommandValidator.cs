using System.Globalization;
using GUI.ContentDefinitions.Commands;

namespace GUI.App.Source.ConsoleSubsystem.Parser
{
    /// <summary>
    /// Represents a set of methods to validate command.
    /// </summary>
    internal class CommandValidator
    {
        /// <summary>
        /// Validates the specified command by checking number of arguments and their types.
        /// </summary>
        /// <param name="command">The command to validate.</param>
        /// <param name="definition">The container of command definitions.</param>
        /// <returns>True if the command is valid, otherwise false.</returns>
        public bool Validate(RawCommand command, CommandDefinition definition)
        {
            if (command.Arguments.Count != definition.Arguments.Count)
            {
                return false;
            }

            for (int i = 0; i < command.Arguments.Count; i++)
            {
                var commandArgument = command.Arguments[i];
                var definitionArgument = definition.Arguments[i];

                if (!TryParseToType(commandArgument, definitionArgument.Type))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the specified value is convertible to the specified type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="type">The type (string/int/bool/float).</param>
        /// <exception cref="TypeNotFoundException">Thrown when the specified type is not recognized.</exception>
        /// <returns>True if the specified value is convertible to the specified type, otherwise false.</returns>
        private bool TryParseToType(string value, string type)
        {
            var result = false;

            switch (type)
            {
                case "string": { result = true; break; }
                case "int": { result = int.TryParse(value, out _); break; }
                case "bool": { result = bool.TryParse(value, out _); break; }
                case "float": { result = float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _); break; }
                default: { throw new TypeNotFoundException(); }
            }
            
            return result;
        }
    }
}
