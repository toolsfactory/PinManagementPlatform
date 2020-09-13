using System;

namespace PinPlatform.Common.DataModels.Caching
{
    public class VerifyAttempts
    {
        public DateTime? LastFailedAttempt { get; set; }
        public byte FailedAttemptsCount { get; set; }
    }
}
