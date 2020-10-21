using System;
using System.Runtime.Serialization;

namespace PinPlatform.Domain.Exceptions
{
    public class DomainInvalidOperationException : DomainException
    {
        protected DomainInvalidOperationException()
        {
        }

        protected DomainInvalidOperationException(string? message) : base(message)
        {
        }
        public DomainInvalidOperationException(ErrorCodes errorCode) : base(errorCode)
        {
        }

        public DomainInvalidOperationException(ErrorCodes errorCode, string? message) : base(errorCode, message)
        {
        }

        public DomainInvalidOperationException(ErrorCodes errorCode, string? message, Exception? innerException) : base(errorCode, message, innerException)
        {
        }

        protected DomainInvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
