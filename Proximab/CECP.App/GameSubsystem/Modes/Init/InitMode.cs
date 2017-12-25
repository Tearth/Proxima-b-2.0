using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;
using CECP.App.GameSubsystem.Exceptions;

namespace CECP.App.GameSubsystem.Modes.Init
{
    /// <summary>
    /// Represents the CECP init mode. Engine inits protocol and sends list of supported features.
    /// </summary>
    public class InitMode : CECPModeBase
    {
        private Dictionary<string, bool> _features;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InitMode"/> class.
        /// </summary>
        public InitMode() : base()
        {
            _features = new Dictionary<string, bool>()
            {
                { "ping", true },
                { "setboard", true },
                { "usermove", true },
                { "done", true }
            };

            CommandsManager.AddCommandHandler(CommandType.ProtoVer, ExecuteProtoVerCommand);
            CommandsManager.AddCommandHandler(CommandType.Rejected, ExecuteRejectedCommand);
            CommandsManager.AddCommandHandler(CommandType.New, ExecuteNewCommand);
        }

        /// <summary>
        /// Processes message (done in derivied class) and prepares a response.
        /// </summary>
        /// <param name="command">The command to process.</param>
        public override void ProcessCommand(Command command)
        {
            CommandsManager.Execute(command);
        }

        /// <summary>
        /// Executes ProtoVer command (sends a list of features).
        /// </summary>
        /// <param name="command">The ProtoVer command to execute.</param>
        private void ExecuteProtoVerCommand(Command command)
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

            SendData(featuresBuilder.ToString());
        }

        /// <summary>
        /// Executes Rejected command (throws exception).
        /// </summary>
        /// <param name="command">The Rejected command to execute.</param>
        /// <exception cref="FeatureNotSupportedException">Thrown when feature is not supported by the CECP interface.</exception>
        private void ExecuteRejectedCommand(Command command)
        {
            throw new FeatureNotSupportedException();
        }

        /// <summary>
        /// Executes New command (changes mode to the Game).
        /// </summary>
        /// <param name="command">The New command to execute.</param>
        private void ExecuteNewCommand(Command command)
        {
            ChangeMode(CECPModeType.Game);
        }
    }
}
