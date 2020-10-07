using System;
using System.Collections.Generic;
using System.Text;
using PinPlatform.Common.Exceptions;

namespace PinPlatform.Domain.Exceptions
{
    public class OpCoNotSupportingPinsException : DomainInvalidOperationException
    {
        public OpCoNotSupportingPinsException() : base(Common.ErrorCodes.OpCoWithoutPins)
        {
        }
    }
}
