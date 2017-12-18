using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.FICS.Source.NetworkSubsystem
{
    /// <summary>
    /// Represents a client state.
    /// </summary>
    public class ClientState
    {
        public const int BufferSize = 8192;

        /// <summary>
        /// Gets or sets the client socket.
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        /// Gets or sets the client buffer.
        /// </summary>
        public byte[] Buffer { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientState"/> class.
        /// </summary>
        public ClientState()
        {
            Buffer = new byte[BufferSize];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientState"/> class.
        /// </summary>
        /// <param name="socket">The client socket.</param>
        public ClientState(Socket socket) : this()
        {
            Socket = socket;
        }
    }
}
