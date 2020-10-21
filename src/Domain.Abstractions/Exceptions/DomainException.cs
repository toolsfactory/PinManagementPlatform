using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PinPlatform.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {

        public ErrorCodes ErrorCode { get; private set; }
        protected DomainException()
        {
        }

        protected DomainException(string? message) : base(message)
        {
        }
        public DomainException(ErrorCodes errorCode) : base(ErrorTexts.GetTextForErrorCode(errorCode))
        {
            ErrorCode = errorCode;
        }

        public DomainException(ErrorCodes errorCode, string? message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public DomainException(ErrorCodes errorCode, string? message, Exception? innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        protected DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
