using Core.Boards;
using Core.Commons;
using GUI.Source.BoardSubsystem;
using GUI.Source.BoardSubsystem.Selections;
using GUI.Source.ConsoleSubsystem;
using GUI.Source.ConsoleSubsystem.Parser;
using System;
using System.Collections.Generic;

namespace GUI.Source.GameModeSubsystem.Editor
{
    internal class EditorGameMode : GameModeBase
    {
        BitBoard _bitBoard;

        public EditorGameMode(ConsoleManager consoleManager) : base(consoleManager)
        {
            _bitBoard = new BitBoard();

            consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;
            _board.OnFieldSelection += Board_OnFieldSelection;
            _board.OnPieceMove += Board_OnPieceMove;
        }

        void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;

            switch(command.Type)
            {
                case CommandType.Occupancy: { DrawOccupancy(command); break; }
            }
        }

        void Board_OnFieldSelection(object sender, FieldSelectedEventArgs e)
        {
            
        }

        void Board_OnPieceMove(object sender, PieceMovedEventArgs e)
        {
            _board.MovePiece(e.From, e.To);

            _bitBoard.SyncWithFriendlyBoard(_board.GetFriendlyBoard());

            _board.SetFriendlyBoard(_bitBoard.GetFriendlyBoard());
        }

        void DrawOccupancy(Command command)
        {
            var color = command.GetArgument<string>(0);
            var occupancyArray = new bool[8, 8];

            if(color == "all")
            {
                occupancyArray = _bitBoard.GetOccupancy();
            }
            else
            {
                var colorType = Color.None;
                var colorTypeParseResult = Enum.TryParse(color, true, out colorType);

                if(!colorTypeParseResult)
                {
                    _consoleManager.WriteLine($"$rInvalid color parameter ($R{color}$r)");
                    return;
                }

                occupancyArray = _bitBoard.GetOccupancy(colorType);
            }

            _board.AddExternalSelections(occupancyArray);
        }
    }
}
