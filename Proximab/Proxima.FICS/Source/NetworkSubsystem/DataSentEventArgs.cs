using System;

namespace Proxima.FICS.Source.NetworkSubsystem
{
    public class DataSentEventArgs : EventArgs
    {
        public string Text { get; private set; }

        public DataSentEventArgs(string text)
        {
            Text = text;
        }
    }
}
