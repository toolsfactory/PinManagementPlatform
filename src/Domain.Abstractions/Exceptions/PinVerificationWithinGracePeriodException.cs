namespace PinPlatform.Domain.Exceptions
{
    public class PinVerificationWithinGracePeriodException : DomainInvalidOperationException
    {
        public PinVerificationWithinGracePeriodException(ushort failedAttempts, int secondsToWait) : base(ErrorCodes.WithinGracePeriod)
        {
            FailedAttempts = failedAttempts;
            SecondsToWait = secondsToWait;
        }

        public ushort FailedAttempts { get; }
        public int SecondsToWait { get; }
    }

}
