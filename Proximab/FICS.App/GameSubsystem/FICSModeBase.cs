using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FICS.App.ConfigSubsystem;
using FICS.App.NetworkSubsystem;

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
        public event EventHandler<ChangeModeEventArgs> OnChangeMode;

        /// <summary>
        /// Gets the configuration manager.
        /// </summary>
        protected ConfigManager ConfigManager { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSModeBase"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public FICSModeBase(ConfigManager configManager)
        {
            ConfigManager = configManager;
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
        /// <returns>The response for the message (<see cref="string.Empty"/> if none).</returns>
        public abstract string ProcessMessage(string message);
    }
}
