using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.FICS.Source.ConfigSubsystem;

namespace Proxima.FICS.Source.FICSModes
{
    public class SeekMode : FICSModeBase
    {
        private bool _seekSent;

        public SeekMode(ConfigManager configManager) : base(configManager)
        {
            _seekSent = false;
        }

        public override string ProcessMessage(string message)
        {
            var response = string.Empty;

            if(!_seekSent)
            {
                response = ConfigManager.GetValue<string>("SeekCommand");
                _seekSent = true;
            }

            if(message.Contains("accepts your seek"))
            {
                ChangeMode(FICSModeType.Game);
            }

            return response;
        }
    }
}
