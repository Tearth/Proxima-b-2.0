using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using FICS.App.ConfigSubsystem;

namespace FICS.App.NetworkSubsystem
{
    /// <summary>
    /// Represents a set of methods to manipulate FICS client.
    /// </summary>
    public class FICSClient
    {
        /// <summary>
        /// The event triggered when data from FICS has been received.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> OnDataReceive;

        /// <summary>
        /// The event triggered when data has been sent to FICS.
        /// </summary>
        public event EventHandler<DataSentEventArgs> OnDataSend;

        private ConfigManager _configManager;
        private Socket _socket;
        private ManualResetEvent _connectDone;

        private List<string> _commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSClient"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public FICSClient(ConfigManager configManager)
        {
            _configManager = configManager;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _connectDone = new ManualResetEvent(false);

            _commands = new List<string>
            {
                FICSConstants.LoginCommand,
                FICSConstants.PasswordCommand,
                FICSConstants.Prompt
            };
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
            _socket.BeginSend(byteDataToSend, 0, byteDataToSend.Length, 0, SendCallback, _socket);

            OnDataSend?.Invoke(this, new DataSentEventArgs(text));
        }

        /// <summary>
        /// Connects to FICS.
        /// </summary>
        private void Connect()
        {
            var serverAddress = _configManager.GetValue<string>(FICSConstants.ServerAddressConfigKeyName);
            var serverPort = _configManager.GetValue<int>(FICSConstants.ServerPortConfigKeyName);

            _socket.BeginConnect(serverAddress, serverPort, ConnectCallback, _socket);
            _connectDone.WaitOne();
        }

        /// <summary>
        /// Starts receiving messages.
        /// </summary>
        private void StartReceiving()
        {
            var clientState = new ClientState(_socket);
            _socket.BeginReceive(clientState.Buffer, 0, ClientState.BufferSize, 0, ReceiveCallback, clientState);
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
            clientState.Socket.EndReceive(ar);

            var clientBuffer = Encoding.UTF8.GetString(clientState.Buffer);

            var lines = ParseClientBuffer(clientBuffer);
            foreach (var line in lines)
            {
                OnDataReceive?.Invoke(this, new DataReceivedEventArgs(line));
            }

            clientState.Socket.BeginReceive(clientState.Buffer, 0, ClientState.BufferSize, 0, ReceiveCallback, clientState);
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

        /// <summary>
        /// Splits client buffer to the separate lines (ended by <see cref="FICSConstants.EndOfLine"/> chars). Incomplete
        /// line (without end line chars) is not returned.
        /// </summary>
        /// <param name="clientBuffer">The client buffer to parse/</param>
        /// <returns>The list of separate lines.</returns>
        private List<string> ParseClientBuffer(string clientBuffer)
        {
            var lines = clientBuffer.Split(new[] { FICSConstants.EndOfLine }, StringSplitOptions.None).ToList();
            var linesWithoutUselessData = RemoveUselessData(lines);

            return linesWithoutUselessData;
        }

        /// <summary>
        /// Removes useless data from received lines. FICS is very verbose and send a lot of old and
        /// duplicated data, especially after commands or prompt.
        /// </summary>
        /// <param name="lines">The lines to filter.</param>
        /// <returns>The list of lines without useless data.</returns>
        private List<string> RemoveUselessData(List<string> lines)
        {
            var linesWithoutUselessData = new List<string>();

            foreach (var line in lines)
            {
                var command = _commands.FirstOrDefault(p => line.Contains(p));

                // Because commands received from FICS has space at the end, we must consider this when comparing strings.
                // Lines with commands or prompt that not contains trash are allowed.
                if (command == null || command.Length == line.Length - 1)
                {
                    linesWithoutUselessData.Add(line);
                }
                else
                {
                    var endSign = command.Last();
                    var signPosition = line.IndexOf(endSign);

                    var fixedLine = line.Substring(0, signPosition + 1);
                    linesWithoutUselessData.Add(fixedLine);

                    break;
                }
            }

            return linesWithoutUselessData;
        }
    }
}
