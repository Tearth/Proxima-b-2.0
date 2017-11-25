using System;
using System.Runtime.Serialization;

namespace Proxima.Core.Commons.Exceptions
{
    public class ColorSymbolNotFoundException : Exception
    {
        public ColorSymbolNotFoundException() : base()
        {
        }

        public ColorSymbolNotFoundException(string message) : base(message)
        {
        }

        public ColorSymbolNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ColorSymbolNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
