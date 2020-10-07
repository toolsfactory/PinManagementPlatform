using PinPlatform.Common;
using PinPlatform.Common.Exceptions;

namespace PinPlatform.Domain.Exceptions
{
    public class PinDoesntExistException : DomainUnknownResourceException
    {
        public PinDoesntExistException() : base(ErrorCodes.NoPinHashFound)
        {
        }
    }

}
