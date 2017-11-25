using System;
using System.Runtime.Serialization;

namespace Proxima.Core.Commons.Exceptions
{
    public class PositionOutOfRangeException : Exception
    {
        public PositionOutOfRangeException() : base()
        {
        }

        public PositionOutOfRangeException(string message) : base(message)
        {
        }

        public PositionOutOfRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PositionOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
