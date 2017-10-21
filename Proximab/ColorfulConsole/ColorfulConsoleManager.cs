using ColorfulConsole.Output;
using GUI.ContentDefinitions.Colors;

namespace ColorfulConsole
{
    public class ColorfulConsoleManager
    {
        OutputParser _outputParser;
        ColorOutputPrinter _outputPrinter;

        public ColorfulConsoleManager()
        {
            _outputParser = new OutputParser();
            _outputPrinter = new ColorOutputPrinter();
        }

        public void LoadContent(ColorDefinitionsContainer colorDefinitionsContainer)
        {
            _outputParser.SetColorDefinitions(colorDefinitionsContainer);
        }

        public void WriteLine()
        {
            WriteLine("");
        }

        public void WriteLine(string output)
        {
            var outputChunks = _outputParser.GetOutputChunks(output);
            _outputPrinter.WriteLine(outputChunks);
        }
    }
}
