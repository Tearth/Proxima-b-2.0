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

            ConsoleManager.OnNewCommand += ConsoleManager_OnNewCommand;
            VisualBoard.OnFieldSelection += Board_OnFieldSelection;
            VisualBoard.OnPieceMove += Board_OnPieceMove;
            PromotionWindow.OnPromotionSelection += PromotionWindow_OnPromotionSelection;
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
                var fieldAttackers = VisualBoard.FriendlyBoard.GetFieldAttackers(e.Position);
                VisualBoard.AddExternalSelections(fieldAttackers);
            }
            else
            {
                var movesForPiece = BitBoard.Moves
                    .Where(p => p.From == e.Position)
                    .Select(p => p.To)
                    .ToList();

                VisualBoard.AddExternalSelections(movesForPiece);
            }
        }

        private void Board_OnPieceMove(object sender, PieceMovedEventArgs e)
        {
            var move = BitBoard.Moves.FirstOrDefault(p => p.From == e.From && p.To == e.To);

            if (move == null)
            {
                CalculateBitBoard(new QuietMove(e.From, e.To, e.Piece.Type, e.Piece.Color));
            }
            else if (move is PromotionMove promotionMove)
            {
                var promotionMoves = BitBoard.Moves.Where(p => p.From == move.From && p is PromotionMove).Cast<PromotionMove>();
                PromotionWindow.Display(move.Color, promotionMoves);
            }
            else
            {
                CalculateBitBoard(move);
            }
        }

        private void PromotionWindow_OnPromotionSelection(object sender, PromotionSelectedEventArgs e)
        {
            CalculateBitBoard(e.Move);
            PromotionWindow.Hide();
        }

        private void AddPiece(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);
            var pieceArgument = command.GetArgument<string>(1);
            var fieldArgument = command.GetArgument<string>(2);

            var colorParseResult = Enum.TryParse(colorArgument, true, out Color color);
            if (!colorParseResult)
            {
                ConsoleManager.WriteLine($"$rInvalid color type ($R{color}$r)");
                return;
            }

            var pieceParseResult = Enum.TryParse(pieceArgument, true, out PieceType piece);
            if (!pieceParseResult)
            {
                ConsoleManager.WriteLine($"$rInvalid piece type ($R{piece}$r)");
                return;
            }

            var fieldPosition = PositionConverter.ToPosition(fieldArgument);
            if (fieldPosition == null)
            {
                ConsoleManager.WriteLine($"$rInvalid field ($R{fieldArgument}$r)");
                return;
            }

            VisualBoard.FriendlyBoard.SetPiece(new FriendlyPiece(fieldPosition, piece, color));
            CalculateBitBoard(VisualBoard.FriendlyBoard);
        }

        private void RemovePiece(Command command)
        {
            var fieldArgument = command.GetArgument<string>(0);

            var fieldPosition = PositionConverter.ToPosition(fieldArgument);
            if (fieldPosition == null)
            {
                ConsoleManager.WriteLine($"$rInvalid field ($R{fieldArgument}$r)");
                return;
            }

            VisualBoard.FriendlyBoard.RemovePiece(fieldPosition);
            CalculateBitBoard(VisualBoard.FriendlyBoard);
        }

        private void DoMovesTest(Command command)
        {
            var test = new MovesTest();

            var calculateEndNodesArgument = command.GetArgument<bool>(0);
            var verifyHashArgument = command.GetArgument<bool>(1);
            var depthArgument = command.GetArgument<int>(2);

            var result = test.Run(VisualBoard.FriendlyBoard, depthArgument, calculateEndNodesArgument, verifyHashArgument);
            ConsoleManager.WriteLine();
            ConsoleManager.WriteLine("$wBenchmark result:");
            ConsoleManager.WriteLine($"$wTotal nodes: $g{result.TotalNodes} N");
            ConsoleManager.WriteLine($"$wEnd nodes: $g{result.EndNodes} N");
            ConsoleManager.WriteLine($"$wHash correct: {ColorfulConsoleHelpers.ParseBool(result.Integrity)}");
            ConsoleManager.WriteLine($"$wNodes per second: $c{result.NodesPerSecond / 1000} kN");
            ConsoleManager.WriteLine($"$wTime per node: $c{result.TimePerNode} ns");
            ConsoleManager.WriteLine($"$wTime: $m{result.Time} s");
            ConsoleManager.WriteLine();
        }
    }
}
