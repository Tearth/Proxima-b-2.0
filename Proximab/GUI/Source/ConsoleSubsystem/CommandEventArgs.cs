using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem
{
    internal class CommandEventArgs : EventArgs
    {
        public DateTime Time { get; set; }
    }
}
