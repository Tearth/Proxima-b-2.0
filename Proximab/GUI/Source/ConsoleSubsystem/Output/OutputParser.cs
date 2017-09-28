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
        readonly char[] Separators = { '$' };

        ColorDefinitionsContainer _colorDefinitionsContainer;

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
            var chunks = output.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            foreach(var chunk in chunks)
            {
                var colorSymbol = chunk[0];
                var text = chunk.Remove(0, 1);

                var definition = _colorDefinitionsContainer.Definitions.FirstOrDefault(p => p.Symbol == colorSymbol.ToString());

                ColorType colorType = ColorType.White;
                if(definition != null)
                {
                    colorType = GetColorTypeByName(definition.EnumType);
                }      

                outputChunks.Add(new OutputChunk()
                {
                    Color = colorType,
                    Text = text
                });
            }

            return outputChunks;
        }

        ColorType GetColorTypeByName(string colorName)
        {
            return (ColorType)Enum.Parse(typeof(ColorType), colorName);
        }
    }
}
