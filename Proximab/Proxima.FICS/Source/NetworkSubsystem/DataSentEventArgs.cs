using System;

namespace Proxima.FICS.Source.NetworkSubsystem
{
    /// <summary>
    /// Represents information about DataSent event
    /// </summary>
    public class DataSentEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the sent text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSentEventArgs"/> class.
        /// </summary>
        /// <param name="text">The sent text.</param>
        public DataSentEventArgs(string text)
        {
            Text = text;
        }
    }
}
