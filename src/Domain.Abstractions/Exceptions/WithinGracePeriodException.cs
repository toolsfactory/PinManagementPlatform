namespace PinPlatform.Domain.Exceptions
{
    public class WithinGracePeriodException : DomainInvalidOperationException
    {
        public WithinGracePeriodException(ushort failedAttempts, int secondsToWait) : base(ErrorCodes.WithinGracePeriod)
        {
            FailedAttempts = failedAttempts;
            SecondsToWait = secondsToWait;
        }

        public ushort FailedAttempts { get; }
        public int SecondsToWait { get; }
    }
}
