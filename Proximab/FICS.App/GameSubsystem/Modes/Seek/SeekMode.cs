using System.Collections.Generic;
using System.Linq;
using FICS.App.ConfigSubsystem;

namespace FICS.App.GameSubsystem.Modes.Seek
{
    /// <summary>
    /// Represents the FICS seek mode. It's the transitional mode between authentication and regular game.
    /// Seek command is sent to FICS and we're waiting for the acceptance from another user.
    /// </summary>
    public class SeekMode : FicsModeBase
    {
        private const string SeekCommandConfigKeyName = "SeekCommand";

        private List<string> _acceptanceTokens;
        private bool _seekSent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeekMode"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public SeekMode(ConfigManager configManager) : base(configManager)
        {
            _seekSent = false;

            _acceptanceTokens = new List<string>()
            {
                "accepts your seek"
            };
        }

        /// <summary>
        /// Processes message (sends seek command and waits for acceptance from another user).
        /// </summary>
        /// <param name="message">The message to process.</param>
        public override void ProcessMessage(string message)
        {
            if (!_seekSent)
            {
                var response = ConfigManager.GetValue<string>(SeekCommandConfigKeyName);
                _seekSent = true;

                SendData(response);
            }

            if (_acceptanceTokens.Any(message.Contains))
            {
                ChangeMode(FicsModeType.Game);
            }
        }
    }
}
