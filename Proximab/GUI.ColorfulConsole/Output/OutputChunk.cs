using System;

namespace GUI.ColorfulConsole.Output
{
    /// <summary>
    /// Represents information about the output chunk.
    /// </summary>
    public class OutputChunk
    {
        /// <summary>
        /// Gets the color in which the text will be displayed.
        /// </summary>
        public ConsoleColor Color { get; private set; }

        /// <summary>
        /// Gets the text to display.
        /// </summary>
        public string Text { get; private set; }

        public OutputChunk(ConsoleColor color, string text)
        {
            Color = color;
            Text = text;
        }
    }
}
