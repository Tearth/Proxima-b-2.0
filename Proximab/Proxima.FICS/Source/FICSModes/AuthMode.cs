using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.FICS.Source.ConfigSubsystem;
using Proxima.FICS.Source.NetworkSubsystem;

namespace Proxima.FICS.Source.FICSModes
{
    public class AuthMode : FICSModeBase
    {
        public AuthMode(ConfigManager configManager) : base(configManager)
        {

        }

        public override string ProcessMessage(string message)
        {
            var response = string.Empty;

            if (message.StartsWith("login:"))
            {
                response = ConfigManager.GetValue<string>("Username");
            }

            if (message.StartsWith("password:"))
            {
                response = ConfigManager.GetValue<string>("Password");
            }

            if(message.Contains("Starting FICS session as"))
            {
                ChangeMode(FICSModeType.Auth);
            }

            return response;
        }
    }
}
