using FICS.App.ConfigSubsystem;
using FICS.App.GameSubsystem.Exceptions;
using FICS.App.GameSubsystem.Modes.Auth;
using FICS.App.GameSubsystem.Modes.Game;
using FICS.App.GameSubsystem.Modes.Seek;
using FICS.App.LogSubsystem;

namespace FICS.App.GameSubsystem
{
    /// <summary>
    /// Represents a factory of FICS modes.
    /// </summary>
    public class FICSModeFactory
    {
        private ConfigManager _configManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSModeFactory"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public FICSModeFactory(ConfigManager configManager)
        {
            _configManager = configManager;
        }

        /// <summary>
        /// Creates a new instance of the FICS mode specified in the parameter.
        /// </summary>
        /// <param name="type">The FICS mode.</param>
        /// <returns>The FICS mode instance.</returns>
        public FICSModeBase Create(FICSModeType type)
        {
            switch (type)
            {
                case FICSModeType.Auth: return new AuthMode(_configManager);
                case FICSModeType.Seek: return new SeekMode(_configManager);
                case FICSModeType.Game: return new GameMode(_configManager);
            }

            throw new FICSModeNotFoundException();
        }
    }
}
