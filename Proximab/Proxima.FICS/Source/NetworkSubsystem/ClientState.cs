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
        public const int BufferSize = 256;

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
        /// Converts the client buffer content to the string representation.
        /// </summary>
        /// <returns>The string representation of <see cref="Buffer"/></returns>
        public string GetStringRepresentation()
        {
            return Encoding.UTF8.GetString(Buffer);
        }
    }
}
