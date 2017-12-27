using System;

namespace FICS.App.GameSubsystem
{
    /// <summary>
    /// Represents information about ChangeMode event.
    /// </summary>
    public class ChangeModeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the type of new FICS mode.
        /// </summary>
        public FicsModeType NewModeType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeModeEventArgs"/> class.
        /// </summary>
        /// <param name="newModeType">The new FICS mode.</param>
        public ChangeModeEventArgs(FicsModeType newModeType)
        {
            NewModeType = newModeType;
        }
    }
}
