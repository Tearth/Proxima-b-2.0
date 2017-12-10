using System;
using System.Linq;
using GUI.App.Source.BoardSubsystem;
using GUI.App.Source.CommandsSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.PromotionSubsystem;
using GUI.ColorfulConsole;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Helpers.Tests;

namespace GUI.App.Source.GameModeSubsystem.Editor
{
    /// <summary>
    /// Represents the editor game mode (allows to add/remove pieces, do tests and some other non-typical actions).
    /// </summary>
    public class EditorMode : ModeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorMode"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager instance.</param>
        /// <param name="commandsManager">The commands manager instance.</param>
        public EditorMode(ConsoleManager consoleManager, CommandsManager commandsManager) : base(consoleManager, commandsManager)
        {
            CalculateBitboard(new DefaultFriendlyBoard());
            
            VisualBoard.OnFieldSelection += Board_OnFieldSelection;
            VisualBoard.OnPieceMove += Board_OnPieceMove;
            PromotionWindow.OnPromotionSelection += PromotionWindow_OnPromotionSelection;

            SetCommandHandlers();
        }

        /// <summary>
        /// Adds all command handlers from current class to the commands manager.
        /// </summary>
        protected new void SetCommandHandlers()
        {
            CommandsManager.AddCommandHandler(CommandType.AddPiece, CommandGroup.GameMode, AddPiece);
            CommandsManager.AddCommandHandler(CommandType.RemovePiece, CommandGroup.GameMode, RemovePiece);
            CommandsManager.AddCommandHandler(CommandType.MovesTest, CommandGroup.GameMode, DoMovesTest);
            CommandsManager.AddCommandHandler(CommandType.BestMove, CommandGroup.GameMode, CalculateBestMove);

            base.SetCommandHandlers();
        }

        /// <summary>
        /// The event handler for OnFieldSelection.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Board_OnFieldSelection(object sender, FieldSelectedEventArgs e)
        {
            if (e.Piece == null)
            {
                var fieldAttackers = VisualBoard.FriendlyBoard.GetFieldAttackers(e.Position);
                VisualBoard.AddExternalSelections(fieldAttackers);
            }
            else
            {
                var movesForPiece = Bitboard.Moves
                    .Where(p => p.From == e.Position)
                    .Select(p => p.To)
                    .ToList();

                VisualBoard.AddExternalSelections(movesForPiece);
            }
        }

        /// <summary>
        /// The event handler for OnPieceMove.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Board_OnPieceMove(object sender, PieceMovedEventArgs e)
        {
            var move = Bitboard.Moves.FirstOrDefault(p => p.From == e.From && p.To == e.To);

            if (move == null)
            {
                CalculateBitboard(new QuietMove(e.From, e.To, e.Piece.Type, e.Piece.Color));
            }
            else if (move is PromotionMove promotionMove)
            {
                var promotionMoves = Bitboard.Moves.Where(p => p.From == move.From && p is PromotionMove).Cast<PromotionMove>();
                PromotionWindow.Display(move.Color, promotionMoves);
            }
            else
            {
                CalculateBitboard(move);
            }
        }

        /// <summary>
        /// The event handler for OnPromotionSelection.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void PromotionWindow_OnPromotionSelection(object sender, PromotionSelectedEventArgs e)
        {
            CalculateBitboard(e.Move);
            PromotionWindow.Hide();
        }

        /// <summary>
        /// Adds a piece specified in the command to the visual board and updates bitboard.
        /// </summary>
        /// <param name="command">The AddPiece command.</param>
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
            CalculateBitboard(VisualBoard.FriendlyBoard);
        }

        /// <summary>
        /// Removes piece specified in the command from the visual board and updates bitboard.
        /// </summary>
        /// <param name="command">The RemovePiece command.</param>
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
            CalculateBitboard(VisualBoard.FriendlyBoard);
        }

        /// <summary>
        /// Does moves test with parameters specified in the command.
        /// </summary>
        /// <param name="command">The DoMovesTest command</param>
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

        /// <summary>
        /// Runs AI calculating.
        /// </summary>
        /// <param name="command">The AI command</param>
        private void CalculateBestMove(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);
            var depthArgument = command.GetArgument<int>(1);

            var colorParseResult = Enum.TryParse(colorArgument, true, out Color color);
            if (!colorParseResult)
            {
                ConsoleManager.WriteLine($"$rInvalid color type ($R{color}$r)");
                return;
            }

            var ai = new AICore();
            var aiResult = ai.Calculate(color, new Bitboard(VisualBoard.FriendlyBoard), depthArgument);

            ConsoleManager.WriteLine();
            ConsoleManager.WriteLine("$wAI result:");

            if (aiResult.BestMove == null)
            {
                ConsoleManager.WriteLine("$gMate");
            }
            else
            {
                ConsoleManager.WriteLine($"$wTotal nodes: $g{aiResult.Stats.TotalNodes} N");
                ConsoleManager.WriteLine($"$wEnd nodes: $g{aiResult.Stats.EndNodes} N");
                ConsoleManager.WriteLine($"$wNodes per second: $c{aiResult.NodesPerSecond / 1000} kN");
                ConsoleManager.WriteLine($"$wTime per node: $c{aiResult.TimePerNode} ns");
                ConsoleManager.WriteLine($"$wTime: $m{aiResult.Time} s");
                ConsoleManager.WriteLine();
                ConsoleManager.WriteLine($"$wBest move: $g{aiResult.BestMove.ToString()}");
                ConsoleManager.WriteLine($"$wScore: $m{aiResult.Score}");
            }

            ConsoleManager.WriteLine();
        }
    }
}
