using PinPlatform.Common.DataModels;
using System;
using System.Threading.Tasks;

namespace PinPlatform.Common.Interfaces
{
    public interface IPinHashVerifier
    {
        uint FailedAttemptsCount { get; }
        DateTime LastFailedAttempt { get; }

        Task<(bool Success, ErrorCodes Error)> VerifyPinHashAsync(RequestorInfo requestor, uint? pinType, string pinHash);
    }
}