using GUI.ContentDefinitions.Colors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.ColorfulConsole.Output
{
    /// <summary>
    /// Represents a set of methods to parse the formatted text into the output chunks.
    /// </summary>
    public class OutputParser
    {
        ColorDefinitionsContainer _colorDefinitionsContainer;

        readonly char[] Separators = { '$' };

        /// <summary>
        /// Sets color definitions. Must be called before first use.
        /// </summary>
        /// <param name="colorDefinitionsContainer">Container of color definitions.</param>
        public void SetColorDefinitions(ColorDefinitionsContainer colorDefinitionsContainer)
        {
            _colorDefinitionsContainer = colorDefinitionsContainer;
        }

        /// <summary>
        /// Calculates output chunks by splitting the text and parsing color symbols.
        /// </summary>
        /// <param name="text">Input text</param>
        /// <returns>
        /// The list of chunks (splitted output text with assigned color enums) which can 
        /// be easily printed on the console.
        /// </returns>
        public IList<OutputChunk> GetOutputChunks(string text)
        {
            var outputChunks = new List<OutputChunk>();
            var splittedOutput = text.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            foreach(var chunk in splittedOutput)
            {
                var colorSymbol = chunk[0].ToString();
                var content = chunk.Remove(0, 1);

                var definition = _colorDefinitionsContainer.Definitions.FirstOrDefault(p => p.Symbol == colorSymbol);
                var color = ParseColorValue(definition?.Color);

                outputChunks.Add(new OutputChunk(color, content));
            }

            return outputChunks;
        }

        /// <summary>
        /// Parses the color value to ConsoleColor enum.
        /// </summary>
        /// <param name="color">The color name</param>
        /// <returns>ConsoleColor enum value</returns>
        ConsoleColor ParseColorValue(string color)
        {
            if(color != null)
            {
                return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
            }

            return ConsoleColor.White;
        }
    }
}
