using System;
using FICS.App.ConfigSubsystem;

namespace FICS.App.GameSubsystem
{
    /// <summary>
    /// Represents a base class for all FICS modes.
    /// </summary>
    public abstract class FICSModeBase
    {
        /// <summary>
        /// The event triggered when FICS mode is changing to another.
        /// </summary>
        public event EventHandler<SendDataEventArgs> OnSendData;

        /// <summary>
        /// The event triggered when FICS mode is changing to another.
        /// </summary>
        public event EventHandler<ChangeModeEventArgs> OnChangeMode;

        /// <summary>
        /// Gets the configuration manager.
        /// </summary>
        protected ConfigManager ConfigManager { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSModeBase"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        protected FICSModeBase(ConfigManager configManager)
        {
            ConfigManager = configManager;
        }

        /// <summary>
        /// Send the specified data to the FICS.
        /// </summary>
        /// <param name="text">The text to send.</param>
        public void SendData(string text)
        {
            OnSendData?.Invoke(this, new SendDataEventArgs(text));
        }

        /// <summary>
        /// Changes mode to the specified one.
        /// </summary>
        /// <param name="newModeType">The new FICS mode.</param>
        public void ChangeMode(FICSModeType newModeType)
        {
            OnChangeMode?.Invoke(this, new ChangeModeEventArgs(newModeType));
        }

        /// <summary>
        /// Processes message (done in derivied class) and prepares a response to the FICS server.
        /// </summary>
        /// <param name="message">The message to process.</param>
        public abstract void ProcessMessage(string message);
    }
}
