using System;

namespace CECP.App.GameSubsystem
{
    /// <summary>
    /// Represents information about ChangeMode event.
    /// </summary>
    public class SendDataEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the text to send.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendDataEventArgs"/> class.
        /// </summary>
        /// <param name="text">The text to send.</param>
        public SendDataEventArgs(string text)
        {
            Text = text;
        }
    }
}
