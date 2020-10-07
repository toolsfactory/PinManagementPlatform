using System;
using PinPlatform.Common.Exceptions;

namespace PinPlatform.Domain.Exceptions
{
    public class OpCoUnknownException : DomainUnknownResourceException
    {
        public OpCoUnknownException() : base(Common.ErrorCodes.UnknownOpCo)
        {
        }
    }
}
