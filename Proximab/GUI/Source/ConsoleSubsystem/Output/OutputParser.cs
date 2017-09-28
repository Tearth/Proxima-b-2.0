using ContentDefinitions.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem.Output
{
    internal class OutputParser
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

                ColorType colorType = ColorType.White;
                if(definition != null)
                {
                    colorType = GetColorTypeByEnumValue(definition.EnumValue);
                }

                outputChunks.Add(new OutputChunk(colorType, text));
            }

            return outputChunks;
        }

        ColorType GetColorTypeByEnumValue(string enumValue)
        {
            return (ColorType)Enum.Parse(typeof(ColorType), enumValue);
        }
    }
}
