using System;
using System.Runtime.Serialization;

namespace GUI.App.Source.ConsoleSubsystem.Parser
{
    public class TypeNotFoundException : Exception
    {
        public TypeNotFoundException() : base()
        {
        }

        public TypeNotFoundException(string message) : base(message)
        {
        }

        public TypeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
