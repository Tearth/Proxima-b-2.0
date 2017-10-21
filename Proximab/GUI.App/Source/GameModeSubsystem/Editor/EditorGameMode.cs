using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Positions;
using GUI.App.Source.BoardSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.ConsoleSubsystem.Parser;
using System;
using System.Linq;
using Proxima.Helpers.BoardSubsystem.Persistence;
using Proxima.Helpers.Tests;

namespace GUI.App.Source.GameModeSubsystem.Editor
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
                case CommandType.AddPiece: { AddPiece(command); break; }
                case CommandType.RemovePiece: { RemovePiece(command); break; }
                case CommandType.Occupancy: { DrawOccupancy(command); break; }
                case CommandType.SaveBoard: { SaveBoard(command); break; }
                case CommandType.LoadBoard: { LoadBoard(command); break; }
                case CommandType.MovesTest: { DoMovesTest(command); break; }
                case CommandType.IsCheck: { IsCheck(command); break; }
            }
        }

        void Board_OnFieldSelection(object sender, FieldSelectedEventArgs e)
        {
            if(e.Piece == null)
            {
                var fieldAttackers = _bitBoard.GetFieldAttackers(e.Position);
                _board.AddExternalSelections(fieldAttackers);
            }
            else
            {
                var availableMoves = _bitBoard.GetAvailableMoves();

                var movesForPiece = availableMoves
                    .Where(p => p.From == e.Position)
                    .Select(p => p.To)
                    .ToList();

                _board.AddExternalSelections(movesForPiece);
            }
        }

        void Board_OnPieceMove(object sender, PieceMovedEventArgs e)
        {
            var availableMoves = _bitBoard.GetAvailableMoves();
            var move = availableMoves.FirstOrDefault(p => p.From == e.From && p.To == e.To);

            if(move == null)
            {
                move = new Move(e.From, e.To, e.Piece.Type, e.Piece.Color, MoveType.Quiet);
            }

            _bitBoard = _bitBoard.Move(move);
            _bitBoard.Calculate(CalculationMode.All);

            _board.SetFriendlyBoard(_bitBoard.GetFriendlyBoard());
        }

        void AddPiece(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);
            var pieceArgument = command.GetArgument<string>(1);
            var fieldArgument = command.GetArgument<string>(2);

            var colorParseResult = Enum.TryParse(colorArgument, true, out Color color);
            if (!colorParseResult)
            {
                _consoleManager.WriteLine($"$rInvalid color type ($R{color}$r)");
                return;
            }

            var pieceParseResult = Enum.TryParse(pieceArgument, true, out PieceType piece);
            if (!pieceParseResult)
            {
                _consoleManager.WriteLine($"$rInvalid piece type ($R{piece}$r)");
                return;
            }

            var fieldPosition = PositionConverter.ToPosition(fieldArgument);
            if (fieldPosition == null)
            {
                _consoleManager.WriteLine($"$rInvalid field ($R{fieldArgument}$r)");
                return;
            }

            _board.AddPiece(fieldPosition, new FriendlyPiece(piece, color));

            UpdateBitBoard();
        }

        void RemovePiece(Command command)
        {
            var fieldArgument = command.GetArgument<string>(0);
            
            var fieldPosition = PositionConverter.ToPosition(fieldArgument);
            if (fieldPosition == null)
            {
                _consoleManager.WriteLine($"$rInvalid field ($R{fieldArgument}$r)");
                return;
            }

            _board.GetFriendlyBoard().SetPiece(fieldPosition, null);

            UpdateBitBoard();
        }

        void DrawOccupancy(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);
            var occupancyArray = new bool[8, 8];

            if (colorArgument == "all")
            {
                occupancyArray = _bitBoard.GetFriendlyOccupancy();
            }
            else
            {
                var colorTypeParseResult = Enum.TryParse(colorArgument, true, out Color colorType);
                if (!colorTypeParseResult)
                {
                    _consoleManager.WriteLine($"$rInvalid color parameter ($R{colorArgument}$r)");
                    return;
                }

                occupancyArray = _bitBoard.GetFriendlyOccupancy(colorType);
            }

            _board.AddExternalSelections(occupancyArray);
        }

        void SaveBoard(Command command)
        {
            var boardNameArgument = command.GetArgument<string>(0);

            var boardWriter = new BoardWriter();
            var board = _board.GetFriendlyBoard();

            var path = $"Boards\\{boardNameArgument}.board";
            boardWriter.Write(path, board);
        }

        void LoadBoard(Command command)
        {
            var boardNameArgument = command.GetArgument<string>(0);

            var boardReader = new BoardReader();
            var path = $"Boards\\{boardNameArgument}.board";

            if (!boardReader.BoardExists(path))
            {
                _consoleManager.WriteLine($"$rBoard {path} not found");
                return;
            }

            _board.SetFriendlyBoard(boardReader.Read(path));

            UpdateBitBoard();
        }

        void DoMovesTest(Command command)
        {
            var test = new MovesTest();

            var calculateEndNodesArgument = command.GetArgument<bool>(0);
            var verifyChecksArgument = command.GetArgument<bool>(1);
            var depthArgument = command.GetArgument<int>(2);

            var result = test.Run(Color.White, _board.GetFriendlyBoard(), depthArgument, calculateEndNodesArgument, verifyChecksArgument);
            _consoleManager.WriteLine();
            _consoleManager.WriteLine("$wBenchmark result:");
            _consoleManager.WriteLine($"$wTotal nodes: $g{result.TotalNodes} N");
            _consoleManager.WriteLine($"$wEnd nodes: $g{result.EndNodes} N");
            _consoleManager.WriteLine($"$wNodes per second: $c{result.NodesPerSecond / 1000} kN");
            _consoleManager.WriteLine($"$wTime per node: $c{result.TimePerNode} ns");
            _consoleManager.WriteLine($"$wTime: $m{result.Time} s");
            _consoleManager.WriteLine();
        }

        void IsCheck(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);
            var colorType = Color.White;

            if(colorArgument == "white" || colorArgument == "w")
            {
                colorType = Color.White;
            }
            else if (colorArgument == "black" || colorArgument == "b")
            {
                colorType = Color.Black;
            }
            else
            {
                _consoleManager.WriteLine($"$rInvalid color name");
                return;
            }

            if(_bitBoard.IsCheck(colorType))
            {
                _consoleManager.WriteLine($"$gYES");
            }
            else
            {
                _consoleManager.WriteLine($"$rNO");
            }
        }

        void UpdateBitBoard()
        {
            _bitBoard = new BitBoard(_board.GetFriendlyBoard());
            _bitBoard.Calculate(CalculationMode.All);
        }
    }
}
