using System;

namespace ColorfulConsole.Output
{
    public class OutputChunk
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
