using System;
using System.Collections.Generic;

namespace GUI.ColorfulConsole.Output
{
    public class ColorOutputPrinter
    {
        public ColorOutputPrinter()
        {

        }

        public void Write(IList<OutputChunk> outputChunks)
        {
            WriteWithColor(outputChunks);
        }

        public void WriteLine(IList<OutputChunk> outputChunks)
        {
            WriteWithColor(outputChunks);
            Console.WriteLine();
        }

        void WriteWithColor(IList<OutputChunk> outputChunks)
        {
            foreach(var chunk in outputChunks)
            {
                Console.ForegroundColor = chunk.Color;
                Console.Write(chunk.Text);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
