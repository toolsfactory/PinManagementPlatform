namespace PinPlatform.Domain.Exceptions
{
    public class PinInvalidException : DomainInvalidOperationException
    {
        public PinInvalidException(ushort failedAttempts, int secondsToWait) : base(ErrorCodes.PinHashesNotMatching)
        {
            FailedAttempts = failedAttempts;
            SecondsToWait = secondsToWait;
        }

        public ushort FailedAttempts { get; }
        public int SecondsToWait { get; }
    }
}
