using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;
using CECP.App.GameSubsystem.Exceptions;
using CECP.App.GameSubsystem.Modes;

namespace CECP.App.GameSubsystem
{
    public class CECPModeFactory
    {
        /// <summary>
        /// Creates a new instance of the CECP mode specified in the parameter.
        /// </summary>
        /// <param name="type">The CECP mode.</param>
        /// <returns>The CECP mode instance.</returns>
        public CECPModeBase Create(CECPModeType type)
        {
            switch (type)
            {
                case CECPModeType.Init: return new InitMode();
                case CECPModeType.Game: return new GameMode();
            }

            throw new CECPModeNotFoundException();
        }
    }
}
