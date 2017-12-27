using System;
using System.Runtime.Serialization;

namespace Proxima.Core.Persistence.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a position value cannot be loaded properly.
    /// </summary>
    public class InvalidPositionValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPositionValueException"/> class.
        /// </summary>
        public InvalidPositionValueException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPositionValueException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidPositionValueException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPositionValueException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidPositionValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPositionValueException"/> class.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public InvalidPositionValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
