using GUI.ContentDefinitions.Colors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.ColorfulConsole.Output
{
    public class OutputParser
    {
        ColorDefinitionsContainer _colorDefinitionsContainer;

        readonly char[] Separators = { '$' };

        public OutputParser()
        {

        }

        public void SetColorDefinitions(ColorDefinitionsContainer colorDefinitionsContainer)
        {
            _colorDefinitionsContainer = colorDefinitionsContainer;
        }

        public IList<OutputChunk> GetOutputChunks(string output)
        {
            var outputChunks = new List<OutputChunk>();
            var splittedOutput = output.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            foreach(var chunk in splittedOutput)
            {
                var colorSymbol = chunk[0];
                var text = chunk.Remove(0, 1);

                var definition = _colorDefinitionsContainer.Definitions.FirstOrDefault(p => p.Symbol == colorSymbol.ToString());

                var color = ConsoleColor.White;
                if(definition != null)
                {
                    color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), definition.Color);
                }

                outputChunks.Add(new OutputChunk(color, text));
            }

            return outputChunks;
        }
    }
}
