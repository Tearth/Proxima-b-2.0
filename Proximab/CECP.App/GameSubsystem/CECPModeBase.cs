using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;

namespace CECP.App.GameSubsystem
{
    public abstract class CECPModeBase
    {
        /// <summary>
        /// The event triggered when CECP mode is changing to another.
        /// </summary>
        public event EventHandler<ChangeModeEventArgs> OnChangeMode;

        protected CommandsManager CommandsManager { get; private set; }

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
        /// Processes message (done in derivied class) and prepares a response to the FICS server.
        /// </summary>
        /// <param name="message">The message to process.</param>
        public virtual string ProcessCommand(Command command)
        {
            return CommandsManager.Execute(command);
        }

        private string ExecutePing(Command command)
        {
            var pingNumber = command.GetArgument<int>(0);

            return $"pong {pingNumber}";
        }
    }
}
