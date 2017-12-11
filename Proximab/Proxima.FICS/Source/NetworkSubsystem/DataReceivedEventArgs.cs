using System;

namespace Proxima.FICS.Source.NetworkSubsystem
{
    /// <summary>
    /// Represents information about DataReceived event
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the time of receiving the message.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Gets the received text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="time">The time of receiving the message.</param>
        /// <param name="text">The message content.</param>
        public DataReceivedEventArgs(DateTime time, string text)
        {
            Time = time;
            Text = text;
        }
    }
}
