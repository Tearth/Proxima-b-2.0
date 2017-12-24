using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CECP.App.GameSubsystem.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a CECP feature is not supported by the engine.
    /// </summary>
    public class FeatureNotSupportedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureNotSupportedException"/> class.
        /// </summary>
        public FeatureNotSupportedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureNotSupportedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FeatureNotSupportedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureNotSupportedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public FeatureNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureNotSupportedException"/> class.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public FeatureNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
