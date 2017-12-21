using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECP.App.GameSubsystem
{
    /// <summary>
    /// Represents information about ChangeMode event.
    /// </summary>
    public class ChangeModeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the type of new FICS mode.
        /// </summary>
        public CECPModeType NewModeType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeModeEventArgs"/> class.
        /// </summary>
        /// <param name="newModeType">The new FICS mode.</param>
        public ChangeModeEventArgs(CECPModeType newModeType)
        {
            NewModeType = newModeType;
        }
    }
}
