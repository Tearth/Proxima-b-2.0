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
        /// The event triggered when FICS mode is changing to another.
        /// </summary>
        public event EventHandler<SendDataEventArgs> OnSendData;

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
            CommandsManager.AddCommandHandler(CommandType.Quit, ExecuteQuit);
        }

        /// <summary>
        /// Send the specified data to the CECP.
        /// </summary>
        /// <param name="newModeType">The text to send.</param>
        public void SendData(string text)
        {
            OnSendData?.Invoke(this, new SendDataEventArgs(text));
        }

        /// <summary>
        /// Changes mode to the specified one.
        /// </summary>
        /// <param name="newModeType">The new CECP mode.</param>
        public void ChangeMode(CECPModeType newModeType)
        {
            OnChangeMode?.Invoke(this, new ChangeModeEventArgs(newModeType));
        }

        /// <summary>
        /// Processes message (done in derivied class) and prepares a response.
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        public virtual void ProcessCommand(Command command)
        {
            CommandsManager.Execute(command);
        }

        /// <summary>
        /// Executes Ping command (responds with Pong X where X is a number received with Ping).
        /// </summary>
        /// <param name="command">The New Ping to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecutePing(Command command)
        {
            var pingNumber = command.GetArgument<int>(0);
            SendData($"pong {pingNumber}");
        }

        /// <summary>
        /// Executes Quit command.
        /// </summary>
        /// <param name="command">The New Quit to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteQuit(Command command)
        {
            Environment.Exit(0);
        }
    }
}
