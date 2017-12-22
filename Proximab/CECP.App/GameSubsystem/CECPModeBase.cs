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
            switch(command.Type)
            {
                case CommandType.Ping: return ExecutePing(command);
            }

            return string.Empty;
        }

        private string ExecutePing(Command command)
        {
            var pingNumber = command.GetArgument<int>(0);

            return $"pong {pingNumber}";
        }
    }
}
