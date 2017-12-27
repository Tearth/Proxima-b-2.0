using System;
using System.Runtime.Serialization;

namespace CECP.App.GameSubsystem.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a CECP mode cannot be found.
    /// </summary>
    public class CecpModeNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CecpModeNotFoundException"/> class.
        /// </summary>
        public CecpModeNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CecpModeNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CecpModeNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CecpModeNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CecpModeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CecpModeNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public CecpModeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
