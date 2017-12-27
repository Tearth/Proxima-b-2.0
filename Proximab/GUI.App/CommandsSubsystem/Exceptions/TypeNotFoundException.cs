using System;
using System.Runtime.Serialization;

namespace GUI.App.CommandsSubsystem.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a type specified in the command is not supported.
    /// </summary>
    public class TypeNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeNotFoundException"/> class.
        /// </summary>
        public TypeNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TypeNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public TypeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public TypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
