using System;

namespace Proxima.FICS.Source.GameSubsystem
{
    /// <summary>
    /// Represents information about ChangeMode event.
    /// </summary>
    public class ChangeModeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the type of new FICS mode.
        /// </summary>
        public FICSModeType NewModeType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeModeEventArgs"/> class.
        /// </summary>
        /// <param name="newModeType">The new FICS mode.</param>
        public ChangeModeEventArgs(FICSModeType newModeType)
        {
            NewModeType = newModeType;
        }
    }
}
