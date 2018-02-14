using System.Threading.Tasks;
using GUI.App.CommandsSubsystem;
using GUI.App.ConsoleSubsystem;
using Proxima.Core.AI;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;

namespace GUI.App.GameSubsystem.Modes
{
    /// <summary>
    /// Represents the AI vs AI game mode (allows to play games with only AIs as players).
    /// </summary>
    public class AIvsAIMode : GameModeBase
    {
        private AICore _whiteAI;
        private AICore _blackAI;
        private Color _currentColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIvsAIMode"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager.</param>
        /// <param name="commandsManager">The commands manager.</param>
        public AIvsAIMode(ConsoleManager consoleManager, CommandsManager commandsManager) : base(consoleManager, commandsManager)
        {
            _whiteAI = new AICore();
            _blackAI = new AICore();
            _currentColor = Color.White;

            CalculateBitboard(new DefaultFriendlyBoard(), false);
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

            Task.Run(() =>
            {
                while (true)
                {
                    var currentAI = _currentColor == Color.White ? _whiteAI : _blackAI;

                    var aiResult = currentAI.Calculate(_currentColor, Bitboard, preferredTimeArgument, 0);
                    var enemyColor = ColorOperations.Invert(_currentColor);

                    ConsoleManager.WriteLine();
                    ConsoleManager.WriteLine($"$w{_currentColor}:");
                    ConsoleManager.WriteLine($"$wBest move: $g{aiResult.PVNodes} $w(Score: $m{aiResult.Score}$w)");
                    ConsoleManager.WriteLine($"$wTotal nodes: $g{aiResult.Stats.TotalNodes} N $w(Depth: $m{aiResult.Depth}$w)");
                    ConsoleManager.WriteLine($"$wTime: $m{aiResult.Time} s");
                    ConsoleManager.WriteLine();

                    CalculateBitboard(aiResult.PVNodes[0], false);

                    if (Bitboard.IsStalemate(enemyColor))
                    {
                        ConsoleManager.WriteLine("$gStalemate");
                        break;
                    }

                    if (Bitboard.IsMate(enemyColor))
                    {
                        ConsoleManager.WriteLine("$gMate");
                        break;
                    }

                    _currentColor = enemyColor;
                }
            });
        }
    }
}
