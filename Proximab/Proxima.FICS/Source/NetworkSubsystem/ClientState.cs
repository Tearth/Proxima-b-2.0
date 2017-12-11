using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.FICS.Source.NetworkSubsystem
{
    public class ClientState
    {
        public const int BufferSize = 256;

        public Socket Socket { get; set; }
        public byte[] Buffer { get; set; }

        public ClientState()
        {
            Buffer = new byte[BufferSize];
        }

        public string GetStringRepresentation()
        {
            return Encoding.UTF8.GetString(Buffer);
        }
    }
}
