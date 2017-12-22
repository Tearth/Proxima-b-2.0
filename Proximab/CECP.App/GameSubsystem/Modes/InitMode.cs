using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;
using CECP.App.GameSubsystem.Exceptions;

namespace CECP.App.GameSubsystem.Modes
{
    public class InitMode : CECPModeBase
    {
        private Dictionary<string, bool> _features;

        public InitMode(ConsoleManager consoleManager) : base(consoleManager)
        {
            _features = new Dictionary<string, bool>()
            {
                { "ping", true },
                { "setboard", true },
                { "usermove", true },
                { "done", true }
            };
        }

        public override void ProcessCommand(Command command)
        {
            switch (command.Type)
            {
                case CommandType.ProtoVer:
                {
                    ExecuteProtoVer(command);
                    break;
                }

                case CommandType.Rejected:
                {
                    throw new FeatureNotSupportedException();
                }

                case CommandType.New:
                {
                    ChangeMode(CECPModeType.Game);
                    break;
                }
            }

            base.ProcessCommand(command);
        }

        private void ExecuteProtoVer(Command command)
        {
            var featuresBuilder = new StringBuilder();
            featuresBuilder.Append("feature ");

            foreach (var feature in _features)
            {
                featuresBuilder.Append(feature.Key);
                featuresBuilder.Append("=");
                featuresBuilder.Append(Convert.ToInt32(feature.Value));
                featuresBuilder.Append(" ");
            }

            ConsoleManager.WriteLine(featuresBuilder.ToString());
        }
    }
}
