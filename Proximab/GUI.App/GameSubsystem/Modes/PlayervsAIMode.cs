using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.App.BoardSubsystem;
using GUI.App.CommandsSubsystem;
using GUI.App.ConsoleSubsystem;
using GUI.App.PromotionSubsystem;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.MoveGenerators.Moves;

namespace GUI.App.GameSubsystem.Modes
{
    /// <summary>
    /// Represents the Player vs AI game mode.
    /// </summary>
    public class PlayervsAIMode : GameModeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayervsAIMode"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager.</param>
        /// <param name="commandsManager">The commands manager.</param>
        public PlayervsAIMode(ConsoleManager consoleManager, CommandsManager commandsManager) : base(consoleManager, commandsManager)
        {
            CalculateBitboard(new DefaultFriendlyBoard(), false);

            VisualBoard.OnFieldSelection += Board_OnFieldSelection;
            VisualBoard.OnPieceMove += Board_OnPieceMove;
            PromotionWindow.OnPromotionSelection += PromotionWindow_OnPromotionSelection;

            SetCommandHandlers();
        }

        /// <summary>
        /// Adds all command handlers from current class to the commands manager.
        /// </summary>
        private void SetCommandHandlers()
        {
            CommandsManager.AddCommandHandler(CommandType.RunAIGame, CommandGroup.GameMode, RunGame);
        }

        /// <summary>
        /// Runs AI game.
        /// </summary>
        /// <param name="command">The AI command.</param>
        private void RunGame(Command command)
        {
            
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
                return;
            }

            switch (move)
            {
                case PromotionMove _:
                    {
                        var promotionMoves = Bitboard.Moves.OfType<PromotionMove>().Where(p => p.From == move.From && p.To == move.To);
                        PromotionWindow.Display(move.Color, promotionMoves);
                        break;
                    }

                default:
                    {
                        CalculateBitboard(move, false);
                        break;
                    }
            }
        }

        /// <summary>
        /// The event handler for OnPromotionSelection.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void PromotionWindow_OnPromotionSelection(object sender, PromotionSelectedEventArgs e)
        {
            CalculateBitboard(e.Move, false);
            PromotionWindow.Hide();
        }
    }
}
