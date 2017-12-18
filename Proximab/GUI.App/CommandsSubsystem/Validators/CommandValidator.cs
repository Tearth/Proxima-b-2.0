using System.Collections.Generic;
using System.Globalization;
using GUI.App.CommandsSubsystem.Exceptions;
using GUI.App.CommandsSubsystem.Parsers;
using GUI.ContentDefinitions.Commands;

namespace GUI.App.CommandsSubsystem.Validators
{
    /// <summary>
    /// Represents a set of methods to validate command.
    /// </summary>
    public class CommandValidator
    {
        private Dictionary<string, ValidationHandlerDelegate> _validationHandlers;

        /// <summary>
        /// Delegate method for validation handlers.
        /// </summary>
        /// <param name="value">The command to validate.</param>
        /// <returns>True if command is valid, otherwise false.</returns>
        private delegate bool ValidationHandlerDelegate(string value);

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandValidator"/> class.
        /// </summary>
        public CommandValidator()
        {
            _validationHandlers = new Dictionary<string, ValidationHandlerDelegate>();

            SetValidationHandlers();
        }

        /// <summary>
        /// Validates the specified command by checking a number of arguments and their types.
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
        /// Adds all command handlers from the current class to the commands manager.
        /// </summary>
        private void SetValidationHandlers()
        {
            _validationHandlers.Add("string", (value) => { return true; });
            _validationHandlers.Add("int", TryParseToInt);
            _validationHandlers.Add("bool", TryParseToBool);
            _validationHandlers.Add("float", TryParseToFloat);
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
            if (!_validationHandlers.ContainsKey(type))
            {
                throw new TypeNotFoundException();
            }

            return _validationHandlers[type].Invoke(value);
        }

        /// <summary>
        /// Checks if the value type is convertible to int type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True is the specified value is convertible to int, otherwise false.</returns>
        private bool TryParseToInt(string value)
        {
            return int.TryParse(value, out _);
        }

        /// <summary>
        /// Checks if the value type is convertible to bool type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True is the specified value is convertible to bool, otherwise false.</returns>
        private bool TryParseToBool(string value)
        {
            return bool.TryParse(value, out _);
        }

        /// <summary>
        /// Checks if the value type is convertible to float type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True is the specified value is convertible to float, otherwise false.</returns>
        private bool TryParseToFloat(string value)
        {
            return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }
    }
}
