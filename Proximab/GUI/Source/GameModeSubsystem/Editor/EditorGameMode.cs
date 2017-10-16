using Core.Boards;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using Core.Commons.Positions;
using GUI.Source.Benchmarks;
using GUI.Source.BoardSubsystem;
using GUI.Source.BoardSubsystem.Persistence;
using GUI.Source.ConsoleSubsystem;
using GUI.Source.ConsoleSubsystem.Parser;
using System;
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
                case CommandType.AddPiece: { AddPiece(command); break; }
                case CommandType.Occupancy: { DrawOccupancy(command); break; }
                case CommandType.SaveBoard: { SaveBoard(command); break; }
                case CommandType.LoadBoard: { LoadBoard(command); break; }
                case CommandType.Benchmark: { DoBenchmark(command); break; }
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

        void DrawOccupancy(Command command)
        {
            var color = command.GetArgument<string>(0);
            var occupancyArray = new bool[8, 8];

            if (color == "all")
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

        void SaveBoard(Command command)
        {
            var boardWriter = new BoardWriter();
            var board = _board.GetFriendlyBoard();

            var path = $"Boards\\{command.GetArgument<string>(0)}.board";
            boardWriter.Write(path, board);
        }

        void LoadBoard(Command command)
        {
            var boardReader = new BoardReader();
            var path = $"Boards\\{command.GetArgument<string>(0)}.board";

            if (!boardReader.BoardExists(path))
            {
                _consoleManager.WriteLine($"$rBoard {path} not found");
                return;
            }

            _board.SetFriendlyBoard(boardReader.Read(path));

            UpdateBitBoard();
        }

        void DoBenchmark(Command command)
        {
            var benchmark = new MovesGeneratorBenchmark(_consoleManager);
            var depth = command.GetArgument<int>(0);
            var verifyChecks = command.GetArgument<bool>(1);

            benchmark.Run(Color.White, _board.GetFriendlyBoard(), depth, verifyChecks);
        }

        void IsCheck(Command command)
        {
            var color = command.GetArgument<string>(0);
            var colorType = Color.White;

            if(color == "white" || color == "w")
            {
                colorType = Color.White;
            }
            else if (color == "black" || color == "b")
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
