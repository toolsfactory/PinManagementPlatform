using System;
using System.Runtime.Serialization;

namespace PinPlatform.Common.Exceptions
{
    public class DomainUnknownResourceException : DomainException
    {
        protected DomainUnknownResourceException()
        {
        }

        protected DomainUnknownResourceException(string? message) : base(message)
        {
        }
        public DomainUnknownResourceException(ErrorCodes errorCode) : base(errorCode)
        {
        }

        public DomainUnknownResourceException(ErrorCodes errorCode, string? message) : base(errorCode, message)
        {
        }

        public DomainUnknownResourceException(ErrorCodes errorCode, string? message, Exception? innerException) : base(errorCode, message, innerException)
        {
        }

        protected DomainUnknownResourceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
