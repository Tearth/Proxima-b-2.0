using System;

namespace Proxima.FICS.Source.NetworkSubsystem
{
    /// <summary>
    /// Represents information about DataReceived event
    /// </summary>
    public class DataEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the received text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEventArgs"/> class.
        /// </summary>
        /// <param name="text">The message content.</param>
        public DataEventArgs(string text)
        {
            Text = text;
        }
    }
}
