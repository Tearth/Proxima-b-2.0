using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Proxima.FICS.Source.ConfigSubsystem;

namespace Proxima.FICS.Source.NetworkSubsystem
{
    public class FICSClient
    {
        public event EventHandler<DataReceivedEventArgs> OnDataReceive;

        private ConfigManager _configManager;
        private Socket _socket;

        private ManualResetEvent _connectDone = new ManualResetEvent(false);

        public FICSClient(ConfigManager configManager)
        {
            _configManager = configManager;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void OpenSession()
        {
            Connect();
            StartReceiving();
        }

        private void Connect()
        {
            var serverAddress = _configManager.GetValue<string>("ServerAddress");
            var serverPort = _configManager.GetValue<int>("ServerPort");

            _socket.BeginConnect(serverAddress, serverPort, new AsyncCallback(ConnectCallback), _socket);
            _connectDone.WaitOne();
        }

        private void StartReceiving()
        {
            var clientState = new ClientState();
            clientState.Socket = _socket;

            _socket.BeginReceive(clientState.Buffer, 0, ClientState.BufferSize, 0, new AsyncCallback(ReceiveCallback), clientState);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            _socket.EndConnect(ar);
            _connectDone.Set();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var clientState = (ClientState)ar.AsyncState; 
            var bytesRead = clientState.Socket.EndReceive(ar);

            var time = DateTime.Now;
            var text = clientState.GetStringRepresentation();

            OnDataReceive?.Invoke(this, new DataReceivedEventArgs(time, text));

            clientState.Socket.BeginReceive(clientState.Buffer, 0, ClientState.BufferSize, 0, new AsyncCallback(ReceiveCallback), clientState);
        }
    }
}
