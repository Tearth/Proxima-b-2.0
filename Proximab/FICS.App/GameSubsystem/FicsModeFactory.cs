using FICS.App.ConfigSubsystem;
using FICS.App.GameSubsystem.Exceptions;
using FICS.App.GameSubsystem.Modes.Auth;
using FICS.App.GameSubsystem.Modes.Game;
using FICS.App.GameSubsystem.Modes.Seek;

namespace FICS.App.GameSubsystem
{
    /// <summary>
    /// Represents a factory of FICS modes.
    /// </summary>
    public class FicsModeFactory
    {
        private ConfigManager _configManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="FicsModeFactory"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public FicsModeFactory(ConfigManager configManager)
        {
            _configManager = configManager;
        }

        /// <summary>
        /// Creates a new instance of the FICS mode specified in the parameter.
        /// </summary>
        /// <param name="type">The FICS mode.</param>
        /// <returns>The FICS mode instance.</returns>
        public FicsModeBase Create(FicsModeType type)
        {
            switch (type)
            {
                case FicsModeType.Auth: return new AuthMode(_configManager);
                case FicsModeType.Seek: return new SeekMode(_configManager);
                case FicsModeType.Game: return new GameMode(_configManager);
            }

            throw new FicsModeNotFoundException();
        }
    }
}
