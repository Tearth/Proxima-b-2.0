﻿using CECP.App.GameSubsystem.Exceptions;
using CECP.App.GameSubsystem.Modes.Game;
using CECP.App.GameSubsystem.Modes.Init;

namespace CECP.App.GameSubsystem
{
    /// <summary>
    /// Represents a factory of CECP modes.
    /// </summary>
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
