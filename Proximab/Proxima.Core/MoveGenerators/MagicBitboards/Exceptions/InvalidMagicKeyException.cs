using System;
using System.Runtime.Serialization;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a magic key is invalid and cannot be used.
    /// </summary>
    public class InvalidMagicKeyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMagicKeyException"/> class.
        /// </summary>
        public InvalidMagicKeyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMagicKeyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidMagicKeyException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMagicKeyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidMagicKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMagicKeyException"/> class.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public InvalidMagicKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
