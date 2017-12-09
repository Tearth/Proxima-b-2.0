using System.Threading.Tasks;
using GUI.App.Source.CommandsSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;

namespace GUI.App.Source.GameModeSubsystem.Editor
{
    internal class AIvsAIMode : ModeBase
    {
        private AICore _ai;
        private Color _currentColor;

        public AIvsAIMode(ConsoleManager consoleManager, CommandsManager commandsManager) : base(consoleManager, commandsManager)
        {
            _ai = new AICore();
            _currentColor = Color.White;

            CalculateBitboard(new DefaultFriendlyBoard());
            SetCommandHandlers();
        }

        protected override void SetCommandHandlers()
        {
            CommandsManager.AddCommandHandler(CommandType.RunAIGame, CommandGroup.GameMode, RunAIGame);
            base.SetCommandHandlers();
        }

        public override void Logic()
        {
            base.Logic();
        }

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
