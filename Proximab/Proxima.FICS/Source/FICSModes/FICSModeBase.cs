using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.FICS.Source.ConfigSubsystem;

namespace Proxima.FICS.Source.FICSModes
{
    public abstract class FICSModeBase
    {
        protected ConfigManager ConfigManager { get; private set; }

        public FICSModeBase(ConfigManager configManager)
        {
            ConfigManager = configManager;
        }

        public abstract void ProcessMessage(string message);
    }
}
