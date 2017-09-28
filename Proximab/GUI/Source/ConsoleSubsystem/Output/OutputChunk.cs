using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem.Output
{
    internal class OutputChunk
    {
        public ConsoleColor Color { get; set; }
        public string Text { get; set; }

        public OutputChunk()
        {

        }

        public OutputChunk(ConsoleColor color, string text)
        {
            Color = color;
            Text = text;
        }
    }
}
