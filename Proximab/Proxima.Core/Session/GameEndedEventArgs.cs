using System;
using Proxima.Core.Commons;

namespace Proxima.Core.Session
{
    /// <summary>
    /// Represents information about DataReceived event
    /// </summary>
    public class GameEndedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the game result.
        /// </summary>
        public GameResult GameResult { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEndedEventArgs"/> class.
        /// </summary>
        /// <param name="gameResult">The game result.</param>
        public GameEndedEventArgs(GameResult gameResult)
        {
            GameResult = gameResult;
        }
    }
}
