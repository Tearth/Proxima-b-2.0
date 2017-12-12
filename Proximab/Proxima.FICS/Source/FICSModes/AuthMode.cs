using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.FICS.Source.ConfigSubsystem;

namespace Proxima.FICS.Source.FICSModes
{
    public class AuthMode : FICSModeBase
    {
        public AuthMode(ConfigManager configManager) : base(configManager)
        {

        }

        public override void ProcessMessage(string message)
        {
            if (message.StartsWith("login:"))
            {
                SendUsername();
            }

            if (message.StartsWith("password:"))
            {
                SendPassword();
            }
        }

        /// <summary>
        /// Sends username to the server.
        /// </summary>
        private void SendUsername()
        {
            var username = ConfigManager.GetValue<string>("Username");
            _ficsClient.Send($"{username}");
        }

        /// <summary>
        /// Sends passwrd to the server.
        /// </summary>
        private void SendPassword()
        {
            var password = ConfigManager.GetValue<string>("Password");
            _ficsClient.Send($"{password}");
        }
    }
}
