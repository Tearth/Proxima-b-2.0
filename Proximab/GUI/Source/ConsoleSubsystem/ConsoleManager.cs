﻿using ContentDefinitions.Colors;
using ContentDefinitions.Commands;
using GUI.Source.ConsoleSubsystem.Output;
using GUI.Source.ConsoleSubsystem.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem
{
    internal class ConsoleManager : ICommandHandler
    {
        public event EventHandler<CommandEventArgs> OnNewCommand;

        Task _consoleLoop;
        CommandParser _commandParser;
        CommandValidator _commandValidator;
        OutputParser _outputParser;
        ColorOutputPrinter _outputPrinter;

        CommandDefinitionsContainer _commandDefinitionsContainer;
        ColorDefinitionsContainer _colorDefinitionsContainer;

        public ConsoleManager()
        {
            _consoleLoop = new Task(() => Loop());

            _commandParser = new CommandParser();
            _commandValidator = new CommandValidator();
            _outputParser = new OutputParser();
            _outputPrinter = new ColorOutputPrinter();
        }

        public void SetCommandDefinitions(CommandDefinitionsContainer commandDefinitionsContainer, 
                                          ColorDefinitionsContainer colorDefinitionsContainer)
        {
            _commandDefinitionsContainer = commandDefinitionsContainer;
            _colorDefinitionsContainer = colorDefinitionsContainer;

            _outputParser.SetColorDefinitions(colorDefinitionsContainer);
        }

        public void Run()
        {
            _consoleLoop.Start();
        }

        public void WriteLine(string output)
        {
            var outputChunks = _outputParser.GetOutputChunks(output);
            _outputPrinter.WriteLine(outputChunks);
        }

        public void HandleCommand(Command command)
        {
            switch(command.Type)
            {
                case CommandType.Colors: { WriteColorsList(); break; }
            }
        }

        void Loop()
        {
            while(true)
            {
                ProcessCommand(Console.ReadLine());
            }
        }

        void ProcessCommand(string input)
        {
            var rawCommand = _commandParser.Parse(input.ToLower().Trim());
            if(rawCommand == null)
            {
                WriteEmptyCommandMessage();
                return;
            }

            var definition = _commandDefinitionsContainer.Definitions
                .FirstOrDefault(p => p.Name.ToLower().Trim() == rawCommand.Name);

            if(definition == null)
            {
                WriteCommandNotFoundMessage(input);
                return;
            }

            var validationResult = _commandValidator.Validate(rawCommand, definition);
            if(!validationResult)
            {
                WriteInvalidCommandFormatMessage(input);
                return;
            }

            var enumType = (CommandType)Enum.Parse(typeof(CommandType), definition.EnumType);
            var commandEventArgs = new CommandEventArgs()
            {
                Time = DateTime.Now,
                Command = new Command()
                {
                    Type = enumType,
                    Arguments = rawCommand.Arguments
                }
            };

            OnNewCommand?.Invoke(this, commandEventArgs);
        }

        void WriteEmptyCommandMessage()
        {
            WriteLine("$rEmpty command");
        }

        void WriteCommandNotFoundMessage(string command)
        {
            WriteLine($"$rCommand not found: {command}");
        }

        void WriteInvalidCommandFormatMessage(string command)
        {
            WriteLine($"$rInvalid command format: {command}");
        }

        void WriteColorsList()
        {
            WriteLine($"$wAvailable colors ({_colorDefinitionsContainer.Definitions.Count}):");

            foreach (var colorDefinition in _colorDefinitionsContainer.Definitions)
            {
                WriteLine($"$w - ${colorDefinition.Symbol}{colorDefinition.Color} - {colorDefinition.Symbol}");
            }
        }
    }
}
