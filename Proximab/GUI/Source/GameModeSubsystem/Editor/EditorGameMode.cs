using Core.Boards;
using Core.Commons;
using Core.Commons.Colors;
using GUI.Source.BoardSubsystem;
using GUI.Source.BoardSubsystem.Selections;
using GUI.Source.ConsoleSubsystem;
using GUI.Source.ConsoleSubsystem.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Source.GameModeSubsystem.Editor
{
    internal class EditorGameMode : GameModeBase
    {
        BitBoard _bitBoard;

        public EditorGameMode(ConsoleManager consoleManager) : base(consoleManager)
        {
            _bitBoard = new BitBoard();

            _consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;
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
            if(e.PieceType != PieceType.None)
            {
                var pieceColor = ColorOperations.GetPieceColor(e.PieceType);
                var availableMoves = _bitBoard.GetAvailableMoves(pieceColor);

                var movesForPiece = availableMoves
                    .Where(p => p.From == e.Position)
                    .Select(p => p.To)
                    .ToList();

                _board.AddExternalSelections(movesForPiece);
            }  
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
                occupancyArray = _bitBoard.GetFriendlyOccupancy();
            }
            else
            {
                var colorTypeParseResult = Enum.TryParse(color, true, out Color colorType);

                if (!colorTypeParseResult)
                {
                    _consoleManager.WriteLine($"$rInvalid color parameter ($R{color}$r)");
                    return;
                }

                occupancyArray = _bitBoard.GetFriendlyOccupancy(colorType);
            }

            _board.AddExternalSelections(occupancyArray);
        }
    }
}
