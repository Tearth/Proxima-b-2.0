using System;

namespace FICS.App.NetworkSubsystem
{
    /// <summary>
    /// Represents information about DataSent event
    /// </summary>
    public class DataSentEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the sent text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSentEventArgs"/> class.
        /// </summary>
        /// <param name="text">The message content.</param>
        public DataSentEventArgs(string text)
        {
            Text = text;
        }
    }
}
