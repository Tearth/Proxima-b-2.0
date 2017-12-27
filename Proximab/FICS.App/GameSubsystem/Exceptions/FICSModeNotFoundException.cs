using System;
using System.Runtime.Serialization;

namespace FICS.App.GameSubsystem.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a FICS mode cannot be found.
    /// </summary>
    public class FICSModeNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FICSModeNotFoundException"/> class.
        /// </summary>
        public FICSModeNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSModeNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FICSModeNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSModeNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public FICSModeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSModeNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public FICSModeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
