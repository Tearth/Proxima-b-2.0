using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.FICS.Source.ConfigSubsystem;

namespace Proxima.FICS.Source.FICSModes
{
    class GameMode : FICSModeBase
    {
        public GameMode(ConfigManager configManager) : base(configManager)
        {

        }

        public override string ProcessMessage(string message)
        {
            var response = string.Empty;

            return response;
        }
    }
}
