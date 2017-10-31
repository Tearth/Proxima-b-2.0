﻿using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Positions;
using GUI.App.Source.BoardSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.ConsoleSubsystem.Parser;
using System;
using System.Linq;
using Proxima.Helpers.Tests;
using Proxima.Core.Boards.Friendly;
using System.Collections.Generic;
using Proxima.Helpers.Persistence;

namespace GUI.App.Source.GameModeSubsystem.Editor
{
    internal class EditorGameMode : GameModeBase
    {
        BitBoard _bitBoard;

        public EditorGameMode(ConsoleManager consoleManager) : base(consoleManager)
        {
            UpdateBitBoard(new DefaultFriendlyBoard());

            _consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;
            _visualBoard.OnFieldSelection += Board_OnFieldSelection;
            _visualBoard.OnPieceMove += Board_OnPieceMove;
        }

        void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;

            switch(command.Type)
            {
                case CommandType.AddPiece: { AddPiece(command); break; }
                case CommandType.RemovePiece: { RemovePiece(command); break; }
                case CommandType.Occupancy: { DrawOccupancy(command); break; }
                case CommandType.Attacks: { DrawAttacks(command); break; }
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
                var fieldAttackers = _visualBoard.GetFriendlyBoard().GetFieldAttackers(e.Position);
                _visualBoard.AddExternalSelections(fieldAttackers);
            }
            else
            {
                var availableMoves = _bitBoard.GetAvailableMoves();

                var movesForPiece = availableMoves
                    .Where(p => p.From == e.Position)
                    .Select(p => p.To)
                    .ToList();

                _visualBoard.AddExternalSelections(movesForPiece);
            }
        }

        void Board_OnPieceMove(object sender, PieceMovedEventArgs e)
        {
            var availableMoves = _bitBoard.GetAvailableMoves();
            var move = availableMoves.FirstOrDefault(p => p.From == e.From && p.To == e.To);
            
            if(move == null)
            {
                move = new QuietMove(e.From, e.To, e.Piece.Type, e.Piece.Color);

                _bitBoard = _bitBoard.Move(move);
                _bitBoard.Calculate(CalculationMode.All);

                _visualBoard.SetFriendlyBoard(_bitBoard.GetFriendlyBoard());
            }
            else if(move is PromotionMove promotionMove)
            {

            }
            else
            {
                _bitBoard = _bitBoard.Move(move);
                _bitBoard.Calculate(CalculationMode.All);

                _visualBoard.SetFriendlyBoard(_bitBoard.GetFriendlyBoard());
            }
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

            _visualBoard.GetFriendlyBoard().SetPiece(new FriendlyPiece(fieldPosition, piece, color));
            UpdateBitBoard(_visualBoard.GetFriendlyBoard());
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

            _visualBoard.GetFriendlyBoard().RemovePiece(fieldPosition);
            UpdateBitBoard(_visualBoard.GetFriendlyBoard());
        }

        void DrawOccupancy(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);

            List<Position> occupancy;
            if (colorArgument == "all")
            {
                occupancy = _visualBoard.GetFriendlyBoard().GetOccupancy();
            }
            else
            {
                var colorTypeParseResult = Enum.TryParse(colorArgument, true, out Color colorType);
                if (!colorTypeParseResult)
                {
                    _consoleManager.WriteLine($"$rInvalid color parameter ($R{colorArgument}$r)");
                    return;
                }

                occupancy = _visualBoard.GetFriendlyBoard().GetOccupancy(colorType);
            }

            _visualBoard.AddExternalSelections(occupancy);
        }

        void DrawAttacks(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);

            List<Position> attacks;
            if (colorArgument == "all")
            {
                attacks = _visualBoard.GetFriendlyBoard().GetAttacks();
            }
            else
            {
                var colorTypeParseResult = Enum.TryParse(colorArgument, true, out Color colorType);
                if (!colorTypeParseResult)
                {
                    _consoleManager.WriteLine($"$rInvalid color parameter ($R{colorArgument}$r)");
                    return;
                }

                attacks = _visualBoard.GetFriendlyBoard().GetAttacks(colorType);
            }

            _visualBoard.AddExternalSelections(attacks);
        }

        void SaveBoard(Command command)
        {
            var boardNameArgument = command.GetArgument<string>(0);

            var boardWriter = new BoardWriter();
            var board = _visualBoard.GetFriendlyBoard();

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

            UpdateBitBoard(boardReader.Read(path));
        }

        void DoMovesTest(Command command)
        {
            var test = new MovesTest();

            var calculateEndNodesArgument = command.GetArgument<bool>(0);
            var verifyChecksArgument = command.GetArgument<bool>(1);
            var depthArgument = command.GetArgument<int>(2);

            var result = test.Run(Color.White, _visualBoard.GetFriendlyBoard(), depthArgument, calculateEndNodesArgument, verifyChecksArgument);
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

        void UpdateBitBoard(FriendlyBoard friendlyBoard)
        {
            _bitBoard = new BitBoard(friendlyBoard);
            _bitBoard.Calculate(CalculationMode.All);

            _visualBoard.SetFriendlyBoard(_bitBoard.GetFriendlyBoard());
        }
    }
}
