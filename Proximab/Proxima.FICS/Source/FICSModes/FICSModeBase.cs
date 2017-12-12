using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.FICS.Source.ConfigSubsystem;
using Proxima.FICS.Source.NetworkSubsystem;

namespace Proxima.FICS.Source.FICSModes
{
    public abstract class FICSModeBase
    {
        public event EventHandler<ChangeModeEventArgs> OnChangeMode;
        protected ConfigManager ConfigManager { get; private set; }
        
        public FICSModeBase(ConfigManager configManager)
        {
            ConfigManager = configManager;
        }

        public void ChangeMode(FICSModeType newModeType)
        {
            OnChangeMode?.Invoke(this, new ChangeModeEventArgs(newModeType));
        }

        public abstract string ProcessMessage(string message);
    }
}
