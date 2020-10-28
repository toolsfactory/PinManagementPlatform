using System;

namespace PinPlatform.Domain.Models
{
    public class PinModel
    {
        public string PinSalt { get; set; }
        public string PinHash { get; set; }
        public bool PinLocked { get; set; }
        public string LockReason { get; set; }
        public DateTime LastFailedAttempt { get; set; }
        public byte FailedAttemptsCount { get; set; }
    }
}