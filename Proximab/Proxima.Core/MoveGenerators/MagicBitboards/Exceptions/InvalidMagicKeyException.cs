using System;
using System.Runtime.Serialization;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Exceptions
{
    public class InvalidMagicKeyException : Exception
    {
        public InvalidMagicKeyException() : base()
        {
        }

        public InvalidMagicKeyException(string message) : base(message)
        {
        }

        public InvalidMagicKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidMagicKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
