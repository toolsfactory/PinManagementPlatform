namespace PinPlatform.Domain.Exceptions
{
    public class PinLockedException : DomainInvalidOperationException
    {
        public PinLockedException(string reason) : base(ErrorCodes.PinLocked)
        {
            Reason = reason;
        }
        public string Reason { get; }
    }
}
