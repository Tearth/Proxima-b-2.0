using FICS.App.ConfigSubsystem;

namespace FICS.App.GameSubsystem.Modes.Auth
{
    /// <summary>
    /// Represents the FICS authentication mode (olny once per session).
    /// </summary>
    public class AuthMode : FicsModeBase
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
        public override void ProcessMessage(string message)
        {
            if (message.StartsWith(FicsConstants.LoginCommand))
            {
                SendData(ProcessLoginCommand());
            }
            else if (message.StartsWith(FicsConstants.PasswordCommand))
            {
                SendData(ProcessPasswordCommand());
            }
            else if (message.Contains(StartingSessionCommand))
            {
                ProcessNewGameSession();
            }
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
            ChangeMode(FicsModeType.Seek);
        }
    }
}
