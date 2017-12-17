using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Proxima.FICS.Source.ConfigSubsystem;

namespace Proxima.FICS.Source.NetworkSubsystem
{
    /// <summary>
    /// Represents a set of methods to manipulate FICS client.
    /// </summary>
    public class FICSClient
    {
        /// <summary>
        /// The event triggered when data from FICS has been received.
        /// </summary>
        public event EventHandler<DataEventArgs> OnDataReceive;

        /// <summary>
        /// The event triggered when data has been sent to FICS.
        /// </summary>
        public event EventHandler<DataEventArgs> OnDataSend;

        private ConfigManager _configManager;
        private Socket _socket;

        private ManualResetEvent _connectDone = new ManualResetEvent(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSClient"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public FICSClient(ConfigManager configManager)
        {
            _configManager = configManager;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        /// <summary>
        /// Opens FICS session and starts receiving messages.
        /// </summary>
        public void OpenSession()
        {
            Connect();
            StartReceiving();
        }

        /// <summary>
        /// Sends the specified text to FICS.
        /// </summary>
        /// <param name="text">The text to send.</param>
        public void Send(string text)
        {
            var byteDataToSend = Encoding.ASCII.GetBytes(text + FICSConstants.EndOfLine);
            _socket.BeginSend(byteDataToSend, 0, byteDataToSend.Length, 0, new AsyncCallback(SendCallback), _socket);

            OnDataSend?.Invoke(this, new DataEventArgs(text));
        }

        /// <summary>
        /// Connects to FICS.
        /// </summary>
        private void Connect()
        {
            var serverAddress = _configManager.GetValue<string>("ServerAddress");
            var serverPort = _configManager.GetValue<int>("ServerPort");

            _socket.BeginConnect(serverAddress, serverPort, new AsyncCallback(ConnectCallback), _socket);
            _connectDone.WaitOne();
        }

        /// <summary>
        /// Starts receiving messages.
        /// </summary>
        private void StartReceiving()
        {
            var clientState = new ClientState(_socket);
            _socket.BeginReceive(clientState.Buffer, 0, ClientState.BufferSize, 0, new AsyncCallback(ReceiveCallback), clientState);
        }

        /// <summary>
        /// Callback for BeginReceive method.
        /// </summary>
        /// <param name="ar">The async result.</param>
        private void ConnectCallback(IAsyncResult ar)
        {
            _socket.EndConnect(ar);
            _connectDone.Set();
        }

        /// <summary>
        /// Callback for BeginReceive method.
        /// </summary>
        /// <param name="ar">The async result.</param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            var clientState = (ClientState)ar.AsyncState;
            var bytesRead = clientState.Socket.EndReceive(ar);

            clientState.BufferString.Append(Encoding.UTF8.GetString(clientState.Buffer));

            var clientBuffer = clientState.BufferString.ToString();
            var lines = ParseClientBuffer(clientBuffer);

            foreach (Match line in lines)
            {
                var text = line.Groups["Text"].Value;
                var textWithoutEndline = text.Substring(0, text.Length - 2);

                clientState.BufferString.Remove(0, text.Length);

                OnDataReceive?.Invoke(this, new DataEventArgs(textWithoutEndline));
            }

            clientState.Socket.BeginReceive(clientState.Buffer, 0, ClientState.BufferSize, 0, new AsyncCallback(ReceiveCallback), clientState);
        }

        /// <summary>
        /// Callback for BeginSend method.
        /// </summary>
        /// <param name="ar">The async result.</param>
        private static void SendCallback(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }

        private MatchCollection ParseClientBuffer(string clientBuffer)
        {
            return Regex.Matches(clientBuffer, $@"(?<Text>.*({FICSConstants.EndOfLine}))", RegexOptions.Multiline);
        }

        /// <summary>
        /// Checks if the specified response from FICS means telnet command.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <returns>True if the specified text is telnet command, otherwise false.</returns>
        private bool IsCommandOrPrompt(string text, out string prefix)
        {
            prefix = string.Empty;

            foreach (var commandPrefix in _commands)
            {
                if (text.StartsWith(commandPrefix))
                {
                    prefix = commandPrefix;
                    return true;
                }
            }

            return false;
        }
    }
}
