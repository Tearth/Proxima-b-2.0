using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GUI.App.CommandsSubsystem;
using GUI.App.ConsoleSubsystem;
using Proxima.Core.AI;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.OpeningBook;
using Proxima.Core.Session;

namespace GUI.App.GameSubsystem.Modes
{
    /// <summary>
    /// Represents the AI vs AI game mode (allows to play games with only AIs as players).
    /// </summary>
    public class AIvsAIMode : GameModeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AIvsAIMode"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager.</param>
        /// <param name="commandsManager">The commands manager.</param>
        public AIvsAIMode(ConsoleManager consoleManager, CommandsManager commandsManager) : base(consoleManager, commandsManager)
        {
            SetCommandHandlers();
        }

        /// <summary>
        /// Adds all command handlers from current class to the commands manager.
        /// </summary>
        private void SetCommandHandlers()
        {
            CommandsManager.AddCommandHandler(CommandType.RunAIGame, CommandGroup.GameMode, RunAIGame);
        }

        /// <summary>
        /// Runs AI game.
        /// </summary>
        /// <param name="command">The AI command.</param>
        private void RunAIGame(Command command)
        {
            var preferredTimeArgument = command.GetArgument<float>(0);

            var whiteAI = new AICore();
            var blackAI = new AICore();
            var currentColor = Color.White;

            var history = new List<Move>();
            var openingBook = new OpeningBookProvider();

            CalculateBitboard(new DefaultFriendlyBoard(), false);

            Task.Run(() =>
            {
                while (true)
                {
                    var currentAI = currentColor == Color.White ? whiteAI : blackAI;
                    var enemyColor = ColorOperations.Invert(currentColor);

                    Move moveToApply;
                    var openingBookMove = openingBook.GetMoveFromBook(history);

                    if (openingBookMove != null)
                    {
                        moveToApply = Bitboard.Moves.First(p =>
                            p.From == openingBookMove.From && p.To == openingBookMove.To);
                    }
                    else
                    {
                        var aiResult = currentAI.Calculate(currentColor, Bitboard, preferredTimeArgument, 0);
                        moveToApply = aiResult.PVNodes[0];

                        ConsoleManager.WriteLine();
                        ConsoleManager.WriteLine($"$w{currentColor}:");
                        ConsoleManager.WriteLine($"$wBest move: $g{aiResult.PVNodes} $w(Score: $m{aiResult.Score}$w)");
                        ConsoleManager.WriteLine($"$wTotal nodes: $g{aiResult.Stats.TotalNodes} N $w(Depth: $m{aiResult.Depth}$w)");
                        ConsoleManager.WriteLine($"$wTime: $m{aiResult.Time} s");
                        ConsoleManager.WriteLine();
                    }

                    CalculateBitboard(moveToApply, false);

                    if (Bitboard.IsStalemate(enemyColor))
                    {
                        ConsoleManager.WriteLine("$gStalemate, wins!");
                        break;
                    }

                    if (Bitboard.IsThreefoldRepetition())
                    {
                        ConsoleManager.WriteLine("$gThreefold repetition!");
                        break;
                    }

                    if (Bitboard.IsMate(enemyColor))
                    {
                        ConsoleManager.WriteLine($"$gMate, {currentColor} wins!");
                        break;
                    }

                    currentColor = enemyColor;
                    history.Add(moveToApply);
                }
            });
        }
    }
}
