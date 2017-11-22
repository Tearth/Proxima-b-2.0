using System;
using System.Linq;
using GUI.App.Source.BoardSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.ConsoleSubsystem.Parser;
using GUI.App.Source.PromotionSubsystem;
using GUI.ColorfulConsole;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Positions;
using Proxima.Helpers.Tests;

namespace GUI.App.Source.GameModeSubsystem.Editor
{
    internal class EditorGameMode : GameModeBase
    {
        public EditorGameMode(ConsoleManager consoleManager) : base(consoleManager)
        {
            CalculateBitBoard(new DefaultFriendlyBoard());

            _consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;
            _visualBoard.OnFieldSelection += Board_OnFieldSelection;
            _visualBoard.OnPieceMove += Board_OnPieceMove;
            _promotionWindow.OnPromotionSelection += PromotionWindow_OnPromotionSelection;
        }

        private void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;

            switch (command.Type)
            {
                case CommandType.AddPiece: { AddPiece(command); break; }
                case CommandType.RemovePiece: { RemovePiece(command); break; }
                case CommandType.MovesTest: { DoMovesTest(command); break; }
            }
        }

        private void Board_OnFieldSelection(object sender, FieldSelectedEventArgs e)
        {
            if (e.Piece == null)
            {
                var fieldAttackers = _visualBoard.GetFriendlyBoard().GetFieldAttackers(e.Position);
                _visualBoard.AddExternalSelections(fieldAttackers);
            }
            else
            {
                var movesForPiece = _bitBoard.Moves
                    .Where(p => p.From == e.Position)
                    .Select(p => p.To)
                    .ToList();

                _visualBoard.AddExternalSelections(movesForPiece);
            }
        }

        private void Board_OnPieceMove(object sender, PieceMovedEventArgs e)
        {
            var move = _bitBoard.Moves.FirstOrDefault(p => p.From == e.From && p.To == e.To);

            if (move == null)
            {
                CalculateBitBoard(new QuietMove(e.From, e.To, e.Piece.Type, e.Piece.Color));
            }
            else if (move is PromotionMove promotionMove)
            {
                var promotionMoves = _bitBoard.Moves.Where(p => p.From == move.From && p is PromotionMove).Cast<PromotionMove>();
                _promotionWindow.Display(move.Color, promotionMoves);
            }
            else
            {
                CalculateBitBoard(move);
            }
        }

        private void PromotionWindow_OnPromotionSelection(object sender, PromotionSelectedEventArgs e)
        {
            CalculateBitBoard(e.Move);
            _promotionWindow.Hide();
        }

        private void AddPiece(Command command)
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
            CalculateBitBoard(_visualBoard.GetFriendlyBoard());
        }

        private void RemovePiece(Command command)
        {
            var fieldArgument = command.GetArgument<string>(0);

            var fieldPosition = PositionConverter.ToPosition(fieldArgument);
            if (fieldPosition == null)
            {
                _consoleManager.WriteLine($"$rInvalid field ($R{fieldArgument}$r)");
                return;
            }

            _visualBoard.GetFriendlyBoard().RemovePiece(fieldPosition);
            CalculateBitBoard(_visualBoard.GetFriendlyBoard());
        }

        private void DoMovesTest(Command command)
        {
            var test = new MovesTest();

            var calculateEndNodesArgument = command.GetArgument<bool>(0);
            var verifyHashArgument = command.GetArgument<bool>(1);
            var depthArgument = command.GetArgument<int>(2);

            var result = test.Run(_visualBoard.GetFriendlyBoard(), depthArgument, calculateEndNodesArgument, verifyHashArgument);
            _consoleManager.WriteLine();
            _consoleManager.WriteLine("$wBenchmark result:");
            _consoleManager.WriteLine($"$wTotal nodes: $g{result.TotalNodes} N");
            _consoleManager.WriteLine($"$wEnd nodes: $g{result.EndNodes} N");
            _consoleManager.WriteLine($"$wHash correct: {ColorfulConsoleHelpers.ParseBool(result.Integrity)}");
            _consoleManager.WriteLine($"$wNodes per second: $c{result.NodesPerSecond / 1000} kN");
            _consoleManager.WriteLine($"$wTime per node: $c{result.TimePerNode} ns");
            _consoleManager.WriteLine($"$wTime: $m{result.Time} s");
            _consoleManager.WriteLine();
        }
    }
}
