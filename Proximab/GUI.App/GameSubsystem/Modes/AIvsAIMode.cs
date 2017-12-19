using System.Threading.Tasks;
using GUI.App.CommandsSubsystem;
using GUI.App.ConsoleSubsystem;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;

namespace GUI.App.GameSubsystem.Modes
{
    /// <summary>
    /// Represents the AI vs AI game mode (allows to play games with only AIs as players).
    /// </summary>
    public class AIvsAIMode : GameModeBase
    {
        private AICore _ai;
        private Color _currentColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIvsAIMode"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager.</param>
        /// <param name="commandsManager">The commands manager.</param>
        public AIvsAIMode(ConsoleManager consoleManager, CommandsManager commandsManager) : base(consoleManager, commandsManager)
        {
            _ai = new AICore();
            _currentColor = Color.White;

            CalculateBitboard(new DefaultFriendlyBoard());
            SetCommandHandlers();
        }

        /// <summary>
        /// Processes all logic related to the base game mode.
        /// </summary>
        public override void Logic()
        {
            base.Logic();
        }

        /// <summary>
        /// Adds all command handlers from current class to the commands manager.
        /// </summary>
        protected override void SetCommandHandlers()
        {
            CommandsManager.AddCommandHandler(CommandType.RunAIGame, CommandGroup.GameMode, RunAIGame);
            base.SetCommandHandlers();
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
                    var aiResult = _ai.Calculate(_currentColor, new Bitboard(VisualBoard.FriendlyBoard), preferredTimeArgument);

                    ConsoleManager.WriteLine();
                    ConsoleManager.WriteLine($"$w{_currentColor}:");

                    if (aiResult.BestMove == null)
                    {
                        ConsoleManager.WriteLine("$gMate");
                        break;
                    }
                    else
                    {
                        ConsoleManager.WriteLine($"$wBest move: $g{aiResult.BestMove.ToString()} $w(Score: $m{aiResult.Score}$w)");
                        ConsoleManager.WriteLine($"$wTotal nodes: $g{aiResult.Stats.TotalNodes} N $w(Depth: $m{aiResult.Depth}$w)");
                        ConsoleManager.WriteLine($"$wTime: $m{aiResult.Time} s");
                    }

                    ConsoleManager.WriteLine();

                    CalculateBitboard(aiResult.BestMove);
                    _currentColor = ColorOperations.Invert(_currentColor);
                }
            });
        }
    }
}
