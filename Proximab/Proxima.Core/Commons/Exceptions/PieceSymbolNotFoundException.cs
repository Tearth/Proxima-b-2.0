using System;
using System.Runtime.Serialization;

namespace Proxima.Core.Commons.Exceptions
{
    public class PieceSymbolNotFoundException : Exception
    {
        public PieceSymbolNotFoundException() : base()
        {

        }

        public PieceSymbolNotFoundException(string message) : base(message)
        {

        }

        public PieceSymbolNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public PieceSymbolNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
