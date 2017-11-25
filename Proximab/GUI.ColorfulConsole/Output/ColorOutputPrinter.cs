using System;
using System.Collections.Generic;

namespace GUI.ColorfulConsole.Output
{
    /// <summary>
    /// Represents a set of methods for writing a colored text on the user console.
    /// </summary>
    public class ColorOutputPrinter
    {
        /// <summary>
        /// Writes the content specified in the outputChunks parameter with the appropriate colors.
        /// </summary>
        /// <param name="outputChunks">The list of chunks to write.</param>
        public void Write(IList<OutputChunk> outputChunks)
        {
            WriteWithColor(outputChunks);
        }

        /// <summary>
        /// Does the same as Write method, but in a new line.
        /// </summary>
        /// <param name="outputChunks">List of chunks to write.</param>
        public void WriteLine(IList<OutputChunk> outputChunks)
        {
            WriteWithColor(outputChunks);
            Console.WriteLine();
        }

        /// <summary>
        /// Iterates through the outputChunks list and writes a text with the specified colors.
        /// </summary>
        /// <param name="outputChunks">The list of chunks to write.</param>
        private void WriteWithColor(IList<OutputChunk> outputChunks)
        {
            foreach (var chunk in outputChunks)
            {
                Console.ForegroundColor = chunk.Color;
                Console.Write(chunk.Text);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
