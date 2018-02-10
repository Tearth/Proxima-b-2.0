using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpeningBookGenerator.App.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a move notation is invalid and cannot be parsed.
    /// </summary>
    public class InvalidMoveNotationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMoveNotationException"/> class.
        /// </summary>
        public InvalidMoveNotationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMoveNotationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidMoveNotationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMoveNotationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidMoveNotationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMoveNotationException"/> class.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public InvalidMoveNotationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
