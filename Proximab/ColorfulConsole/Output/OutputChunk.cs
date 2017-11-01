using System;

namespace ColorfulConsole.Output
{
    public class OutputChunk
    {
        public ConsoleColor Color { get; private set; }
        public string Text { get; private set; }

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
