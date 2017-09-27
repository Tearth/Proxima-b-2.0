using ContentDefinitions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem.Parser
{
    internal class CommandValidator
    {
        public bool Validate(Command command, CommandDefinition definition)
        {
            if (command.Arguments.Count != definition.Arguments.Count)
                return false;
            
            for(int i=0; i<command.Arguments.Count; i++)
            {
                var commandArg = command.Arguments[i];
                var definitionArgType = definition.Arguments[i];

                if (!TryParseToType(commandArg, definitionArgType))
                    return false;
            }

            return true;
        }

        bool TryParseToType(string value, string type)
        {
            var result = false;

            switch (type)
            {
                case "string": { result = true; break; }
                case "int":  { result = Int32.TryParse(value, out _); break; }
                case "boolean": { result = Boolean.TryParse(value, out _); break; }
                case "float": { result = Single.TryParse(value, out _); break; }
            }

            return result;
        }
    }
}
