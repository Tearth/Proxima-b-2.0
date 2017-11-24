using System.Collections.Generic;
using System.Globalization;
using GUI.App.Source.CommandsSubsystem.Exceptions;
using GUI.App.Source.CommandsSubsystem.Parsers;
using GUI.ContentDefinitions.Commands;

namespace GUI.App.Source.CommandsSubsystem.Validators
{
    /// <summary>
    /// Represents a set of methods to validate command.
    /// </summary>
    internal class CommandValidator
    {
        private delegate bool ValidationHandlerDelegate(string value);
        private Dictionary<string, ValidationHandlerDelegate> _validationHandlers;

        public CommandValidator()
        {
            _validationHandlers = new Dictionary<string, ValidationHandlerDelegate>();

            SetValidationHandlers();
        }

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

        private bool TryParseToInt(string value)
        {
            return int.TryParse(value, out _);
        }

        private bool TryParseToBool(string value)
        {
            return bool.TryParse(value, out _);
        }

        private bool TryParseToFloat(string value)
        {
            return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }
    }
}
