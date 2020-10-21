namespace PinPlatform.Domain.Exceptions
{
    public class OpCoNotSupportingPinsException : DomainInvalidOperationException
    {
        public OpCoNotSupportingPinsException() : base(ErrorCodes.OpCoWithoutPins)
        {
        }
    }
}
