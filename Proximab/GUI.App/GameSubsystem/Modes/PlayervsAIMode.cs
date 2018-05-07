using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.App.BoardSubsystem;
using GUI.App.CommandsSubsystem;
using GUI.App.ConsoleSubsystem;
using GUI.App.PromotionSubsystem;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.OpeningBook;

namespace GUI.App.GameSubsystem.Modes
{
    /// <summary>
    /// Represents the Player vs AI game mode.
    /// </summary>
    public class PlayervsAIMode : GameModeBase
    {
        private Color _playerColor;
        private float _preferredTime;
        private int _helperTasksCount;
        private AICore _ai;
        private List<Move> _history;
        private OpeningBookProvider _openingBook;
        private Color _currentColor;
        private bool _done;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayervsAIMode"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager.</param>
        /// <param name="commandsManager">The commands manager.</param>
        public PlayervsAIMode(ConsoleManager consoleManager, CommandsManager commandsManager) : base(consoleManager, commandsManager)
        {
            _ai = new AICore();
            _history = new List<Move>();
            _openingBook = new OpeningBookProvider();
            _currentColor = Color.White;
            _done = false;

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
            CommandsManager.AddCommandHandler(CommandType.RunGame, CommandGroup.GameMode, RunGame);
        }

        /// <summary>
        /// Runs AI game.
        /// </summary>
        /// <param name="command">The AI command.</param>
        private void RunGame(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);
            var preferredTimeArgument = command.GetArgument<float>(1);
            var helperTasksCount = command.GetArgument<int>(2);

            var colorParseResult = Enum.TryParse(colorArgument, true, out Color color);
            if (!colorParseResult)
            {
                ConsoleManager.WriteLine($"$rInvalid color type ($R{color}$r)");
                return;
            }

            _playerColor = color;
            _preferredTime = preferredTimeArgument;
            _helperTasksCount = helperTasksCount;

            if (_playerColor == Color.Black)
            {
                MoveAI();
            }
        }

        /// <summary>
        /// The event handler for OnFieldSelection.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Board_OnFieldSelection(object sender, FieldSelectedEventArgs e)
        {
            if (!_done && _currentColor == _playerColor && e.Piece != null && e.Piece.Color == _currentColor)
            {
                var enemyColor = ColorOperations.Invert(_currentColor);

                var movesForPiece = Bitboard.Moves
                    .Where(p => p.From == e.Position && !Bitboard.Moves.Any(q => p.Piece == PieceType.King && 
                                                                                 q.Color == enemyColor && q.To == p.To))
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
            if (!_done && _currentColor == _playerColor && e.Piece != null && e.Piece.Color == _currentColor)
            {
                var enemyColor = ColorOperations.Invert(_currentColor);

                var move = Bitboard.Moves.FirstOrDefault(p => p.From == e.From && p.To == e.To);

                if (move == null)
                {
                    return;
                }

                var testBitboard = Bitboard.Move(move);
                testBitboard.Calculate(false);

                if (testBitboard.IsCheck(_currentColor))
                {
                    ConsoleManager.WriteLine("$RInvalid move");
                    return;
                }

                switch (move)
                {
                    case PromotionMove _:
                    {
                        var promotionMoves = Bitboard.Moves.OfType<PromotionMove>().Where(p => p.From == move.From && 
                                                                                               p.To == move.To);
                        PromotionWindow.Display(move.Color, promotionMoves);

                        break;
                    }

                    default:
                    {
                        CalculateBitboard(move, false);
                        break;
                    }
                }

                CheckIfGameHasEnded(Bitboard);

                _currentColor = ColorOperations.Invert(_currentColor);
                _history.Add(move);

                MoveAI();
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

        /// <summary>
        /// Does an AI move.
        /// </summary>
        private void MoveAI()
        {
            Task.Run(() =>
            {
                var openingBookMove = _openingBook.GetMoveFromBook(_history);
                var enemyColor = ColorOperations.Invert(_currentColor);

                Move moveToApply = null;

                if (openingBookMove != null)
                {
                    moveToApply = Bitboard.Moves.First(p =>
                        p.From == openingBookMove.From && p.To == openingBookMove.To);
                }
                else
                {
                    var aiResult = _ai.Calculate(_currentColor, Bitboard, _preferredTime, _helperTasksCount);
                    moveToApply = aiResult.PVNodes[0];

                    ConsoleManager.WriteLine();
                    ConsoleManager.WriteLine($"$w{_currentColor}:");
                    ConsoleManager.WriteLine($"$wBest move: $g{aiResult.PVNodes} $w(Score: $m{aiResult.Score}$w)");
                    ConsoleManager.WriteLine($"$wTotal nodes: $g{aiResult.Stats.TotalNodes} N $w(Depth: $m{aiResult.Depth}$w)");
                    ConsoleManager.WriteLine($"$wTime: $m{aiResult.Time} s");
                    ConsoleManager.WriteLine();
                }

                CalculateBitboard(moveToApply, false);

                CheckIfGameHasEnded(Bitboard);

                _currentColor = enemyColor;
                _history.Add(moveToApply);
            });
        }

        /// <summary>
        /// Checks if the specified bitboard is finished or not.
        /// </summary>
        /// <param name="bitboard">The bitboard to check.</param>
        private void CheckIfGameHasEnded(Bitboard bitboard)
        {
            var enemyColor = ColorOperations.Invert(_currentColor);

            if (Bitboard.IsStalemate(enemyColor))
            {
                ConsoleManager.WriteLine("$gStalemate, wins!");
                _done = true;
                return;
            }

            if (Bitboard.IsThreefoldRepetition())
            {
                ConsoleManager.WriteLine("$gThreefold repetition!");
                _done = true;
                return;
            }

            if (Bitboard.IsMate(enemyColor))
            {
                ConsoleManager.WriteLine($"$gMate, {_currentColor} wins!");
                _done = true;
                return;
            }
        }
    }
}
