using System;

namespace Proxima.FICS.Source.FICSModes
{
    public class ChangeModeEventArgs : EventArgs
    {
        public FICSModeType NewModeType { get; private set; }

        public ChangeModeEventArgs(FICSModeType newModeType)
        {
            NewModeType = newModeType;
        }
    }
}
