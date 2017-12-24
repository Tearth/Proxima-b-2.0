using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;

namespace CECP.App.GameSubsystem
{
    /// <summary>
    /// Represents a base class for all CECP modes.
    /// </summary>
    public abstract class CECPModeBase
    {
        /// <summary>
        /// The event triggered when CECP mode is changing to another.
        /// </summary>
        public event EventHandler<ChangeModeEventArgs> OnChangeMode;

        /// <summary>
        /// Gets the commands manager.
        /// </summary>
        protected CommandsManager CommandsManager { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CECPModeBase"/> class.
        /// </summary>
        public CECPModeBase()
        {
            CommandsManager = new CommandsManager();

            CommandsManager.AddCommandHandler(CommandType.Ping, ExecutePing);
        }

        /// <summary>
        /// Changes mode to the specified one.
        /// </summary>
        /// <param name="newModeType">The new FICS mode.</param>
        public void ChangeMode(CECPModeType newModeType)
        {
            OnChangeMode?.Invoke(this, new ChangeModeEventArgs(newModeType));
        }

        /// <summary>
        /// Processes message (done in derivied class) and prepares a response.
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <returns>The reponse (<see cref="string.Empty"/> if none).</returns>
        public virtual string ProcessCommand(Command command)
        {
            return CommandsManager.Execute(command);
        }

        /// <summary>
        /// Executes Ping command (responds with Pong X where X is a number received with Ping).
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <returns>The reponse (<see cref="string.Empty"/> if none).</returns>
        private string ExecutePing(Command command)
        {
            var pingNumber = command.GetArgument<int>(0);

            return $"pong {pingNumber}";
        }
    }
}
