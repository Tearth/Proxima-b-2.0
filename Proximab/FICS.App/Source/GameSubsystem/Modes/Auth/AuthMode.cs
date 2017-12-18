using FICS.App.Source.ConfigSubsystem;
using FICS.App.Source.LogSubsystem;

namespace FICS.App.Source.GameSubsystem.Modes.Auth
{
    /// <summary>
    /// Represents the FICS authentication mode (olny once per session).
    /// </summary>
    public class AuthMode : FICSModeBase
    {
        private const string UsernameConfigKeyName = "Username";
        private const string PasswordConfigKeyName = "Password";

        private const string StartingSessionCommand = "Starting FICS session as";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMode"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public AuthMode(ConfigManager configManager) : base(configManager)
        {
        }

        /// <summary>
        /// Processes message (sends username/password and changes mode if FICS response is positive).
        /// </summary>
        /// <param name="message">The message to process.</param>
        /// <returns>The response for the message (<see cref="string.Empty"/> if none).</returns>
        public override string ProcessMessage(string message)
        {
            var response = string.Empty;

            if (message.StartsWith(FICSConstants.LoginCommand))
            {
                response = ProcessLoginCommand();
            }
            else if (message.StartsWith(FICSConstants.PasswordCommand))
            {
                response = ProcessPasswordCommand();
            }
            else if (message.Contains(StartingSessionCommand))
            {
                ProcessNewGameSession();
            }

            return response;
        }

        /// <summary>
        /// Processes login command.
        /// </summary>
        /// <returns>The response to the login command.</returns>
        private string ProcessLoginCommand()
        {
            return ConfigManager.GetValue<string>(UsernameConfigKeyName);
        }

        /// <summary>
        /// Processes password command.
        /// </summary>
        /// <returns>The response to the password command.</returns>
        private string ProcessPasswordCommand()
        {
            return ConfigManager.GetValue<string>(PasswordConfigKeyName);
        }

        /// <summary>
        /// Processes new game session command.
        /// </summary>
        private void ProcessNewGameSession()
        {
            ChangeMode(FICSModeType.Seek);
        }
    }
}
