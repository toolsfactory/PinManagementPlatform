using System;
using PinPlatform.Common;
using PinPlatform.Common.Exceptions;

namespace PinPlatform.Domain.Exceptions
{
    public class PinNotCompliantToRulesException : DomainInvalidOperationException
    {
        public PinNotCompliantToRulesException(ErrorCodes errorCode) : base(errorCode)
        {
        }
    }
}
