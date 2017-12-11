using System.Threading.Tasks;
using GUI.App.Source.CommandsSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;

namespace GUI.App.Source.GameModeSubsystem.Editor
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
            Task.Run(() =>
            {
                while (true)
                {
                    var aiResult = _ai.Calculate(_currentColor, new Bitboard(VisualBoard.FriendlyBoard), 4);

                    ConsoleManager.WriteLine();
                    ConsoleManager.WriteLine("$wAI result:");

                    if (aiResult.BestMove == null)
                    {
                        ConsoleManager.WriteLine("$gMate");
                        break;
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

                    CalculateBitboard(aiResult.BestMove);
                    _currentColor = ColorOperations.Invert(_currentColor);
                }
            });
        }
    }
}
