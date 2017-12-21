using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECP.App.ConsoleSubsystem
{
    public class ConsoleManager
    {
        public void WriteLine(string text)
        {

        }

        public void WaitForCommand()
        {
            var commandText = Console.ReadLine();
        }
    }
}
