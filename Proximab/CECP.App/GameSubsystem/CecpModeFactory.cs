using CECP.App.GameSubsystem.Exceptions;
using CECP.App.GameSubsystem.Modes.Game;
using CECP.App.GameSubsystem.Modes.Init;

namespace CECP.App.GameSubsystem
{
    /// <summary>
    /// Represents a factory of CECP modes.
    /// </summary>
    public class CecpModeFactory
    {
        /// <summary>
        /// Creates a new instance of the CECP mode specified in the parameter.
        /// </summary>
        /// <param name="type">The CECP mode.</param>
        /// <returns>The CECP mode instance.</returns>
        public CecpModeBase Create(CecpModeType type)
        {
            switch (type)
            {
                case CecpModeType.Init: return new InitMode();
                case CecpModeType.Game: return new GameMode();
            }

            throw new CecpModeNotFoundException();
        }
    }
}
