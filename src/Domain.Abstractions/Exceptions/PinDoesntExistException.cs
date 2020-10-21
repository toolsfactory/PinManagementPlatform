using PinPlatform.Common;

namespace PinPlatform.Domain.Exceptions
{
    public class PinDoesntExistException : DomainUnknownResourceException
    {
        public PinDoesntExistException() : base(ErrorCodes.NoPinHashFound)
        {
        }
    }

}
