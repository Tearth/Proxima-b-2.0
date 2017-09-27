using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem
{
    internal class ConsoleManager
    {
        public event EventHandler<CommandEventArgs> OnNewCommand;

        Task consoleLoop;

        public ConsoleManager()
        {
            consoleLoop = new Task(() => Loop());
        }

        public void Run()
        {
            consoleLoop.Start();
        }

        void Loop()
        {
            while(true)
            {
                ProcessCommand(Console.ReadLine());
            }
        }

        void ProcessCommand(string command)
        {

        }
    }
}
