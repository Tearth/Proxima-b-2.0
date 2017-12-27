using System;

namespace FICS.App.NetworkSubsystem
{
    /// <summary>
    /// Represents information about DataReceived event
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the received text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="text">The message content.</param>
        public DataReceivedEventArgs(string text)
        {
            Text = text;
        }
    }
}
