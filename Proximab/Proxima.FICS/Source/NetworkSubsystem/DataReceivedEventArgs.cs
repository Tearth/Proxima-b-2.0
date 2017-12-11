using System;

namespace Proxima.FICS.Source.NetworkSubsystem
{
    public class DataReceivedEventArgs : EventArgs
    {
        public DateTime Time { get; private set; }
        public string Text { get; private set; }

        public DataReceivedEventArgs(DateTime time, string text)
        {
            Time = time;
            Text = text;
        }
    }
}
