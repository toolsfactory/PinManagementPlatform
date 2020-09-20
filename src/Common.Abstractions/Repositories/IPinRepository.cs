using System;
using System.Threading.Tasks;

namespace PinPlatform.Common.Repositories
{
    public interface IPinRepository
    {
        Task<(uint FailedAttemptsCount, DateTime LastFailedAttempt)> GetFailedVerificationsInfoAsync(DataModels.RequestorInfo requestor, uint? pinType);
        Task UpdateFailedVerificationsInfoAsync(DataModels.RequestorInfo requestor, uint? pinType, uint failedAtempts, DateTime lastFailed);
        Task DeleteFailedAttemptsInfoAsync(DataModels.RequestorInfo requestor, uint? pinType);
        Task<string> GetPinHashAsync(DataModels.RequestorInfo requestor, uint? pinType);
        Task SetPinAsync(DataModels.RequestorInfo requestor, uint? pinType, string hash);
    }
}