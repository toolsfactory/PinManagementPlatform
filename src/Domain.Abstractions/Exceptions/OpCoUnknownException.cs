namespace PinPlatform.Domain.Exceptions
{
    public class OpCoUnknownException : DomainUnknownResourceException
    {
        public OpCoUnknownException() : base(ErrorCodes.UnknownOpCo)
        {
        }
    }
}
