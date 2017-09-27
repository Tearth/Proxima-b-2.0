﻿using ContentDefinitions.Commands;
using GUI.Source.ConsoleSubsystem.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem
{
    internal class ConsoleManager
    {
        public event EventHandler<CommandEventArgs> OnNewCommand;

        Task _consoleLoop;
        CommandDefinitionsContainer _commandDefinitionsContainer;
        CommandParser _commandParser;
        CommandValidator _commandValidator;

        public ConsoleManager()
        {
            _consoleLoop = new Task(() => Loop());

            _commandParser = new CommandParser();
            _commandValidator = new CommandValidator();
        }

        public void SetCommandDefinitions(CommandDefinitionsContainer commandDefinitionsContainer)
        {
            _commandDefinitionsContainer = commandDefinitionsContainer;
        }

        public void Run()
        {
            _consoleLoop.Start();
        }

        public void Write(string output)
        {
            Console.WriteLine(output);
        }

        void Loop()
        {
            while(true)
            {
                ProcessCommand(Console.ReadLine());
            }
        }

        void ProcessCommand(string command)
        {
            var rawCommand = _commandParser.Parse(command);
            if(rawCommand == null)
            {
                WriteEmptyCommandMessage();
                return;
            }

            var definition = _commandDefinitionsContainer.Definitions.FirstOrDefault(p => p.Name == rawCommand.Name);
            if(definition == null)
            {
                WriteCommandNotFoundMessage(command);
                return;
            }

            var validationResult = _commandValidator.Validate(rawCommand, definition);
            if(!validationResult)
            {
                WriteInvalidCommandFormatMessage(command);
                return;
            }
        }

        void WriteEmptyCommandMessage()
        {
            Console.WriteLine($"Empty command");
        }

        void WriteCommandNotFoundMessage(string command)
        {
            Console.WriteLine($"Command not found: {command}");
        }

        void WriteInvalidCommandFormatMessage(string command)
        {
            Console.WriteLine($"Invalid command format: {command}");
        }
    }
}
