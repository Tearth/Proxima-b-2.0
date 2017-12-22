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

        public InitMode() : base()
        {
            _features = new Dictionary<string, bool>()
            {
                { "ping", true },
                { "setboard", true },
                { "usermove", true },
                { "done", true }
            };
        }

        public override string ProcessCommand(Command command)
        {
            switch (command.Type)
            {
                case CommandType.ProtoVer: return ExecuteProtoVerCommand(command);
                case CommandType.Rejected: throw new FeatureNotSupportedException();
                case CommandType.New: return ExecuteNewCommand(command);
            }

            return base.ProcessCommand(command);
        }

        private string ExecuteProtoVerCommand(Command command)
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

            return featuresBuilder.ToString();
        }

        private string ExecuteRejectCommand(Command command)
        {
            throw new FeatureNotSupportedException();
        }

        private string ExecuteNewCommand(Command command)
        {
            ChangeMode(CECPModeType.Game);
            return string.Empty;
        }
    }
}
