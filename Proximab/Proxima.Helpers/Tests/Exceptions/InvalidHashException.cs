using System;
using System.Runtime.Serialization;

namespace Proxima.Helpers.Tests.Exceptions
{
    public class InvalidHashException : Exception
    {
        public InvalidHashException() : base()
        {

        }

        public InvalidHashException(string message) : base(message)
        {

        }

        public InvalidHashException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public InvalidHashException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
